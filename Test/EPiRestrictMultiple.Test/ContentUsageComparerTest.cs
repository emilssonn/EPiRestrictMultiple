namespace EPiRestrictMultiple.Test
{
    using System;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="ContentUsageComparerTest"/> class.
    /// </summary>
    [TestClass]
    public class ContentUsageComparerTest
    {
        /// <summary>
        /// Get a <see cref="ContentUsage"/> instance with the specified <see cref="ContentUsage.ContentLink"/> ID.
        /// </summary>
        private readonly Func<int, ContentUsage> _getContentUsage = delegate (int i)
            {
                return new ContentUsage()
                {
                    ContentLink = new ContentReference(i)
                };
            };

        /// <summary>
        /// The _content usage comparer
        /// </summary>
        private readonly ContentUsageComparer _contentUsageComparer = new ContentUsageComparer();

        /// <summary>
        /// Determines whether this instance [can get hash code].
        /// </summary>
        [TestMethod]
		public void CanGetHashCode()
		{
            var contentUsage = _getContentUsage(1);

            var hashCode = _contentUsageComparer.GetHashCode(contentUsage);

            Assert.AreEqual(1.GetHashCode(), hashCode);
		}

        /// <summary>
        /// Determines whether this instance [can get hash code from null].
        /// </summary>
        [TestMethod]
        public void CanGetHashCodeFromNull()
        {
            var hashCode = _contentUsageComparer.GetHashCode(null);

            Assert.AreEqual(0.GetHashCode(), hashCode);
        }

        /// <summary>
        /// Determines whether this instance [can get hash code from null content link].
        /// </summary>
        [TestMethod]
        public void CanGetHashCodeFromNullContentLink()
        {
            var hashCode = _contentUsageComparer.GetHashCode(new ContentUsage()
            {
                ContentLink = null
            });

            Assert.AreEqual(0.GetHashCode(), hashCode);
        }

        /// <summary>
        /// Determines whether this instance [can get equals].
        /// </summary>
        [TestMethod]
        public void CanGetEquals()
        {
            var contentUsage1 = _getContentUsage(1);
            var contentUsage2 = _getContentUsage(1);

            var result = _contentUsageComparer.Equals(contentUsage1, contentUsage2);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Determines whether this instance [can get null equals].
        /// </summary>
        [TestMethod]
        public void CanGetNullEquals()
        {
            var result = _contentUsageComparer.Equals(null, null);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Determines whether this instance [can get single null equal].
        /// </summary>
        [TestMethod]
        public void CanGetSingleNullEqual()
        {
            var contentUsage1 = _getContentUsage(1);

            var result = _contentUsageComparer.Equals(contentUsage1, null);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Determines whether this instance [can get none equals].
        /// </summary>
        [TestMethod]
        public void CanGetNoneEquals()
        {
            var contentUsage1 = _getContentUsage(1);
            var contentUsage2 = _getContentUsage(2);

            var result = _contentUsageComparer.Equals(contentUsage1, contentUsage2);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Determines whether this instance [can get equals with null content links].
        /// </summary>
        [TestMethod]
        public void CanGetEqualsWithNullContentLinks()
        {
            var contentUsage1 = new ContentUsage()
            {
                ContentLink = null
            };

            var contentUsage2 = new ContentUsage()
            {
                ContentLink = null
            };

            var result = _contentUsageComparer.Equals(contentUsage1, contentUsage2);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Determines whether this instance [can get equals with one null content link].
        /// </summary>
        [TestMethod]
        public void CanGetEqualsWithOneNullContentLink()
        {
            var contentUsage1 = new ContentUsage()
            {
                ContentLink = null
            };

            var contentUsage2 = new ContentUsage()
            {
                ContentLink = new ContentReference(1)
            };

            var result = _contentUsageComparer.Equals(contentUsage1, contentUsage2);

            Assert.IsFalse(result);
        }
    }
}
