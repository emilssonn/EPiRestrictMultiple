namespace EPiRestrictMultiple.Sample.Models.Pages
{
    using System.ComponentModel.DataAnnotations;
    using EPiRestrictMultiple;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.DataAnnotations;

    /// <summary>
    /// The <see cref="MaxOneCultureSpecificPage"/> class.
    /// </summary>
    [ContentType(
        DisplayName = "MaxOneCultureSpecificPage",
        GUID = "ff9b5dcc-4518-4947-885d-2d4002f132b0",
        Description = "",
        GroupName = SystemTabNames.Content,
        Order = 300)]
    [RestrictMultiple(Max = 1, IncludeWasteBasket = false)]
    public class MaxOneCultureSpecificPage : PageData
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [CultureSpecific]
        [Display(
            Name = "Title",
            Description = "The title.",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string Title { get; set; }
    }
}