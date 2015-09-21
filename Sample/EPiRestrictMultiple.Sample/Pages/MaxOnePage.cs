namespace EPiRestrictMultiple.Sample.Models.Pages
{
    using System.ComponentModel.DataAnnotations;
    using EPiRestrictMultiple;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.DataAnnotations;

    /// <summary>
    /// The <see cref="MaxOnePage"/> class.
    /// </summary>
    [ContentType(
        DisplayName = "MaxOnePage",
        GUID = "8d3157ad-7ab3-4db6-9c27-5397e4cb4540",
        Description = "",
        GroupName = SystemTabNames.Content,
        Order = 100)]
    [RestrictMultiple(Max = 1)]
    public class MaxOnePage : PageData
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