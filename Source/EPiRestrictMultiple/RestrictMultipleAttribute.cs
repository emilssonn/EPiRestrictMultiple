namespace EPiRestrictMultiple
{
    using System;

    /// <summary>
    /// The <see cref="RestrictMultipleAttribute"/> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RestrictMultipleAttribute : Attribute
    {
        /// <summary>
        /// The _max
        /// </summary>
        private int _max = 1;

        /// <summary>
        /// The _culture specific
        /// </summary>
        private bool _cultureSpecific = true;

        /// <summary>
        /// The _include waste basket
        /// </summary>
        private bool _includeWasteBasket = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestrictMultipleAttribute"/> class.
        /// </summary>
        public RestrictMultipleAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        /// <exception cref="ArgumentException">Max cannot be 0 or less.;Max</exception>
        public int Max
        {
            get
            {
                return _max;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Max cannot be 0 or less.", "Max");
                }
                _max = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [culture specific].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [culture specific]; otherwise, <c>false</c>.
        /// </value>
        public bool CultureSpecific
        {
            get
            {
                return _cultureSpecific;
            }

            set
            {
                _cultureSpecific = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [include waste basket].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [include waste basket]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeWasteBasket
        {
            get
            {
                return _includeWasteBasket;
            }

            set
            {
                _includeWasteBasket = value;
            }
        }
    }
}
