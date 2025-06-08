using System.Collections.Generic;
using Xunit;

namespace AggregateKit.Tests
{
    public class ValueObjectNullTests
    {
        private class NullableVO : ValueObject
        {
            public string? Value { get; }

            public NullableVO(string? value)
            {
                Value = value;
            }

            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return Value;
            }
        }

        [Fact]
        public void ValueObjects_With_Null_Value_Are_Equal_And_GetHashCode_Does_Not_Throw()
        {
            // Arrange
            var vo1 = new NullableVO(null);
            var vo2 = new NullableVO(null);

            // Act & Assert
            Assert.Equal(vo1, vo2);
            var hash1 = vo1.GetHashCode();
            var hash2 = vo2.GetHashCode();
            Assert.Equal(hash1, hash2);
        }
    }
}
