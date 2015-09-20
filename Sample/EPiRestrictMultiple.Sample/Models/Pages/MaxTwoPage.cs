namespace EPiRestrictMultiple.Sample.Models.Pages
{
    using System.ComponentModel.DataAnnotations;
    using EPiRestrictMultiple;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.DataAnnotations;

    /// <summary>
    /// The <see cref="MaxTwoPage"/> class.
    /// </summary>
    [ContentType(
        DisplayName = "MaxTwoPage", 
        GUID = "dbe6b533-2557-485f-bba1-981bb06279b2", 
        Description = "",
        GroupName = SystemTabNames.Content,
        Order = 200)]
    [RestrictMultiple(Max = 2)]
    public class MaxTwoPage : PageData
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