namespace EPiRestrictMultiple
{
    using System.Collections.Generic;
    using EPiServer.DataAbstraction;

    /// <summary>
    /// The <see cref="ContentUsageComparer"/> class.
    /// This comparer is used only in the <see cref="RestrictMultipleAttributeInitialization"/> class.
    /// It compares two <see cref="ContentUsage"/> instances by comparing the <see cref="ContentUsage.ContentLink"/>.ID property.
    /// Null values is the same as an ID with a value of 0.
    /// </summary>
    internal class ContentUsageComparer : IEqualityComparer<ContentUsage>
    {
        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(ContentUsage x, ContentUsage y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(ContentUsage obj)
        {
            if (obj == null || obj.ContentLink == null)
            {
                return 0;
            }

            return obj.ContentLink.ID.GetHashCode();
        }
    }
}
