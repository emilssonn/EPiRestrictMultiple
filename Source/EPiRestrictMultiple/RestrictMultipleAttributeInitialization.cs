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
        /// The _events attached
        /// </summary>
        private bool _eventsAttached = false;

        /// <summary>
        /// The _content events
        /// </summary>
        private readonly Injected<IContentEvents> _contentEvents;

        /// <summary>
        /// The _localization service
        /// </summary>
        private readonly Injected<LocalizationService> _localizationService;

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
            if (_eventsAttached)
            {
                return;
            }

            _contentEvents.Service.CreatingContent += CreatingContent;
            _contentEvents.Service.MovingContent += MovingContent;
            _contentEvents.Service.SavingContent += SavingContent;

            _eventsAttached = true;
        }

        /// <summary>
        /// Uninitializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Uninitialize(InitializationEngine context)
        {
            if (!_eventsAttached)
            {
                return;
            }

            _contentEvents.Service.CreatingContent -= CreatingContent;
            _contentEvents.Service.MovingContent -= MovingContent;
            _contentEvents.Service.SavingContent -= SavingContent;

            _eventsAttached = false;
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
        /// Savings the content.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ContentEventArgs"/> instance containing the event data.</param>
        private void SavingContent(object sender, ContentEventArgs e)
        {
            var type = e.Content.GetOriginalType();

            if (type.IsDefined(typeof(RestrictMultipleAttribute), false))
            {
                var attribute = type.GetCustomAttributes(typeof(RestrictMultipleAttribute), false)[0] as RestrictMultipleAttribute;

                if (!attribute.CultureSpecific)
                {
                    var existingLanguages = type.GetProperty("ExistingLanguages").GetValue(e.Content, null) as IEnumerable<CultureInfo>;
                    if (existingLanguages.Any() && !existingLanguages.Contains(type.GetProperty("Language").GetValue(e.Content, null) as CultureInfo))
                    {
                        var contentType = _contentTypeRepository.Service.Load(e.Content.ContentTypeID);
                        var message = string.Format(_localizationService.Service.GetString("/RestrictMultiple/max", "You can only create {0} instances of the {1} type."),
                            attribute.Max, contentType.LocalizedFullName);
                        e.CancelAction = true;
                        e.CancelReason = message;

                        return;
                    }
                }
            }
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

                var usages = _contentModelUsage.Service.ListContentOfContentType(contentType);

                if (moving)
                {
                    usages = usages.Where(x => x.ContentLink.ID != e.ContentLink.ID).ToList();
                }

                if (attribute.CultureSpecific)
                {
                    usages = usages.Distinct(new ContentUsageComparer()).ToList();
                }

                var totalCount = usages.Count();

                if (!attribute.IncludeWasteBasket)
                {
                    var contents = _contentRepository.Service.GetItems(usages.Select(x => x.ContentLink), LanguageSelector.AutoDetect()).Where(x => x.ParentLink != ContentReference.WasteBasket);

                    totalCount = contents.Count();
                }

                if (totalCount >= attribute.Max)
                {
                    string resourceKey = "/RestrictMultiple/max";
                    string message = "You can only create {0} instances of the {1} type.";

                    if (attribute.CultureSpecific)
                    {
                        resourceKey = "/RestrictMultiple/culturemax";
                        message = "You can only create {0} instances of the {1} type across all languages.";
                    }

                    e.CancelAction = true;
                    e.CancelReason = string.Format(_localizationService.Service.GetString(resourceKey, message), attribute.Max, contentType.LocalizedFullName);

                    return;
                }
            }
        }
    }
}