namespace EPiRestrictMultiple.Sample.Models.Pages
{
    using System.ComponentModel.DataAnnotations;
    using EPiRestrictMultiple;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.DataAnnotations;

    [ContentType(
        DisplayName = "MaxTwoCultureSpecificPage",
        GUID = "9ef63708-5c97-45ff-90a6-be64cd5f7de1",
        Description = "",
        GroupName = SystemTabNames.Content,
        Order = 400)]
    [RestrictMultiple(Max = 2, IncludeWasteBasket = false)]
    public class MaxTwoCultureSpecificPage : PageData
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