﻿namespace EPiRestrictMultiple
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
        /// The _include waste basket
        /// </summary>
        private bool _includeWasteBasket = true;

        /// <summary>
        /// The _disable at maximum
        /// </summary>
        private bool _disableAtMax = true;

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
        /// <exception cref="System.ArgumentException">Max cannot be less than 0.;Max</exception>
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
                    throw new ArgumentException("Max cannot be less than 0.", "Max");
                }
                _max = value;
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

        /// <summary>
        /// Gets or sets a value indicating whether [disable at maximum].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disable at maximum]; otherwise, <c>false</c>.
        /// </value>
        public bool DisableAtMax
        {
            get
            {
                return _disableAtMax;
            }

            set
            {
                _disableAtMax = value;
            }
        }
    }
}
