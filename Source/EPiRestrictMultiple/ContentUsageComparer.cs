namespace EPiRestrictMultiple
{
    using System.Collections.Generic;
    using EPiServer.DataAbstraction;

    /// <summary>
    /// The <see cref="ContentUsageComparer"/> class.
    /// </summary>
    public class ContentUsageComparer : IEqualityComparer<ContentUsage>
    {
        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(ContentUsage x, ContentUsage y)
        {
            return x.ContentLink.ID == y.ContentLink.ID;
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
            if (obj == null)
            {
                return 0;
            }

            return obj.ContentLink.ID.GetHashCode();
        }
    }
}
