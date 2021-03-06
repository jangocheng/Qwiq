﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using Should;

namespace Qwiq.Mapper.Identity
{
    public class SingleDisplayNameContextSpecification : DisplayNameToAliasConverterContextSpecification
    {
        protected internal string DisplayName { get; set; }

        /// <inheritdoc />
        [TestMethod]
        [TestCategory("localOnly")]
        [TestCategory("SOAP")]
        public void Converted_value_contains_a_single_result()
        {
            var val = (string)ConvertedValue;
            val.ShouldNotBeNull();
        }

        /// <inheritdoc />
        public override void When()
        {
            ConvertedValue = TimedAction(() => ValueConverter.Map(DisplayName), "SOAP", "Map");
        }
    }
}