namespace EPiRestrictMultiple
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using EPiServer;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.Framework;
    using EPiServer.Framework.Initialization;
    using EPiServer.Framework.Localization;
    using EPiServer.ServiceLocation;

    /// <summary>
    /// The <see cref="RestrictMultipleAttributeInitialization" /> class.
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule), typeof(ServiceContainerInitialization))]
    public class RestrictMultipleAttributeInitialization : IInitializableModule
    {
        /// <summary>
        /// The _initialized
        /// </summary>
        private bool _initialized = false;

        /// <summary>
        /// The _localization service
        /// </summary>
        private readonly Injected<LocalizationService> _localizationService;

        /// <summary>
        /// The _content events
        /// </summary>
        private readonly Injected<IContentEvents> _contentEvents;

        /// <summary>
        /// The _content type repository
        /// </summary>
        private readonly Injected<IContentTypeRepository> _contentTypeRepository;

        /// <summary>
        /// The _content repository
        /// </summary>
        private readonly Injected<IContentRepository> _contentRepository;

        /// <summary>
        /// The _content model usage
        /// </summary>
        private readonly Injected<IContentModelUsage> _contentModelUsage;

        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Initialize(InitializationEngine context)
        {
            if (_initialized)
            {
                return;
            }

            _contentEvents.Service.CreatingContent += CreatingContent;
            _contentEvents.Service.MovingContent += MovingContent;

            var contentTypes = _contentTypeRepository.Service.List();
            var disableAtMax = false;

            foreach (var contentType in contentTypes)
            {
                if (contentType.ModelType.IsDefined(typeof(RestrictMultipleAttribute), false))
                {
                    var attribute = contentType.ModelType.GetCustomAttributes(typeof(RestrictMultipleAttribute), false)[0] as RestrictMultipleAttribute;
                    if (attribute.DisableAtMax)
                    {
                        disableAtMax = true;
                        break;
                    }
                }
            }

            if (disableAtMax)
            {
                _contentEvents.Service.CreatedContent += CreatedContent;
                _contentEvents.Service.DeletedContent += DeletedContent;
            }

            _initialized = true;
        }

        /// <summary>
        /// Uninitializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Uninitialize(InitializationEngine context)
        {
            if (!_initialized)
            {
                return;
            }

            _contentEvents.Service.CreatingContent -= CreatingContent;
            _contentEvents.Service.MovingContent -= MovingContent;
            _contentEvents.Service.CreatedContent -= CreatedContent;
            _contentEvents.Service.DeletedContent -= DeletedContent;

            _initialized = false;
        }

        /// <summary>
        /// Deleted the content.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DeleteContentEventArgs"/> instance containing the event data.</param>
        private void DeletedContent(object sender, DeleteContentEventArgs e)
        {
            UpdateContent(e, true);
        }

        /// <summary>
        /// Created the content.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ContentEventArgs"/> instance containing the event data.</param>
        private void CreatedContent(object sender, ContentEventArgs e)
        {
            UpdateContent(e, false);
        }

        /// <summary>
        /// Creatings the content.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ContentEventArgs"/> instance containing the event data.</param>
        private void CreatingContent(object sender, ContentEventArgs e)
        {
            ValidateContent(e);
        }

        /// <summary>
        /// Movings the content.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ContentEventArgs"/> instance containing the event data.</param>
        private void MovingContent(object sender, ContentEventArgs e)
        {
            if (e.Content.ParentLink != ContentReference.WasteBasket)
            {
                return;
            }

            ValidateContent(e, true);
        }

        /// <summary>
        /// Validates the content.
        /// </summary>
        /// <param name="e">The <see cref="ContentEventArgs"/> instance containing the event data.</param>
        /// <param name="moving">if set to <c>true</c> [moving].</param>
        private void ValidateContent(ContentEventArgs e, bool moving = false)
        {
            var type = e.Content.GetOriginalType();

            if (type.IsDefined(typeof(RestrictMultipleAttribute), false))
            {
                var attribute = type.GetCustomAttributes(typeof(RestrictMultipleAttribute), false)[0] as RestrictMultipleAttribute;

                var contentType = _contentTypeRepository.Service.Load(e.Content.ContentTypeID);

                var totalCount = GetTotalCount(attribute, contentType, moving, e.ContentLink);

                if (totalCount >= attribute.Max)
                {
                    string resourceKey = "/RestrictMultiple/max";
                    string message = "You can only create {0} instances of the {1} type.";

                    e.CancelAction = true;
                    e.CancelReason = string.Format(_localizationService.Service.GetString(resourceKey, message), attribute.Max, contentType.LocalizedFullName);

                    return;
                }
            }
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="e">The <see cref="ContentEventArgs"/> instance containing the event data.</param>
        /// <param name="availability">if set to <c>true</c> [availability].</param>
        private void UpdateContent(ContentEventArgs e, bool availability)
        {
            var type = e.Content.GetOriginalType();

            if (type.IsDefined(typeof(RestrictMultipleAttribute), false))
            {
                var attribute = type.GetCustomAttributes(typeof(RestrictMultipleAttribute), false)[0] as RestrictMultipleAttribute;

                var contentType = _contentTypeRepository.Service.Load(e.Content.ContentTypeID);

                var totalCount = GetTotalCount(attribute, contentType);

                if (attribute.DisableAtMax &&
                    ((!availability && totalCount >= attribute.Max && contentType.IsAvailable) || (availability && totalCount < attribute.Max && !contentType.IsAvailable)))
                {
                    var clone = (ContentType)contentType.CreateWritableClone();
                    clone.IsAvailable = availability;
                    _contentTypeRepository.Service.Save(clone);
                }
            }
        }

        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="moving">if set to <c>true</c> [moving].</param>
        /// <param name="contentReference">The content reference.</param>
        /// <returns></returns>
        private int GetTotalCount(RestrictMultipleAttribute attribute, ContentType contentType, bool moving = false, ContentReference contentReference = null)
        {
            var usages = _contentModelUsage.Service.ListContentOfContentType(contentType);

            if (moving)
            {
                usages = usages.Where(x => x.ContentLink.ID != contentReference.ID).ToList();
            }

            usages = usages.Distinct(new ContentUsageComparer()).ToList();

            var totalCount = usages.Count();

            if (!attribute.IncludeWasteBasket)
            {
                var contents = _contentRepository.Service.GetItems(usages.Select(x => x.ContentLink), LanguageSelector.AutoDetect()).Where(x => x.ParentLink != ContentReference.WasteBasket);

                totalCount = contents.Count();
            }

            return totalCount;
        }
    }
}