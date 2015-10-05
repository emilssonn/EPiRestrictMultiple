namespace EPiRestrictMultiple.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
	public class RestrictMultipleAttributeTest
	{
        /// <summary>
        /// Determines whether this instance [can create attribute with default values].
        /// </summary>
        [TestMethod]
		public void CanCreateWithDefaultValues()
		{
            var attribute = new RestrictMultipleAttribute();

            Assert.IsTrue(attribute.IncludeWasteBasket);
            Assert.AreEqual(1, attribute.Max);
		}

        /// <summary>
        /// Determines whether this instance [can create attribute with values].
        /// </summary>
        [TestMethod]
        public void CanCreateWithValues()
        {
            var attribute = new RestrictMultipleAttribute()
            {
                IncludeWasteBasket = false,
                Max = 2
            };

            Assert.IsFalse(attribute.IncludeWasteBasket);
            Assert.AreEqual(2, attribute.Max);
        }

        /// <summary>
        /// Determines whether this instance [can not create with negative maximum].
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotCreateWithNegativeMax()
        {
            var attribute = new RestrictMultipleAttribute()
            {
                Max = -1
            };

            Assert.Fail("Should throw");
        }

        /// <summary>
        /// Determines whether this instance [can not create with zero maximum].
        /// </summary>
        [TestMethod]
        public void CanCreateWithZeroMax()
        {
            var attribute = new RestrictMultipleAttribute()
            {
                Max = 0
            };

            Assert.AreEqual(0, attribute.Max);
        }
    }
}
