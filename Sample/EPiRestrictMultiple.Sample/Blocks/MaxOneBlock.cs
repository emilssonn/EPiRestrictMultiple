namespace EPiRestrictMultiple.Sample.Models.Blocks
{
    using System.ComponentModel.DataAnnotations;
    using EPiRestrictMultiple;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.DataAnnotations;

    [ContentType(
        DisplayName = "MaxOneBlock",
        GUID = "1cdf9ead-e41b-43b2-89e0-4f8117f6f86c",
        Description = "",
        GroupName = SystemTabNames.Content,
        Order = 100)]
    [RestrictMultiple(Max = 1)]
    public class MaxOneBlock : BlockData
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