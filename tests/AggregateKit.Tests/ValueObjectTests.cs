using System.Collections.Generic;
using Xunit;

namespace AggregateKit.Tests
{
    public class ValueObjectTests
    {
        private class Money : ValueObject
        {
            public decimal Amount { get; }
            public string Currency { get; }
            
            public Money(decimal amount, string currency)
            {
                Amount = amount;
                Currency = currency;
            }
            
            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return Amount;
                yield return Currency;
            }
        }

        [Fact]
        public void ValueObjects_With_Same_Values_Are_Equal()
        {
            // Arrange
            var money1 = new Money(100, "USD");
            var money2 = new Money(100, "USD");
            
            // Act & Assert
            Assert.Equal(money1, money2);
            Assert.True(money1 == money2);
            Assert.False(money1 != money2);
            Assert.Equal(money1.GetHashCode(), money2.GetHashCode());
        }

        [Fact]
        public void ValueObjects_With_Different_Values_Are_Not_Equal()
        {
            // Arrange
            var money1 = new Money(100, "USD");
            var money2 = new Money(100, "EUR");
            var money3 = new Money(200, "USD");
            
            // Act & Assert
            Assert.NotEqual(money1, money2);
            Assert.NotEqual(money1, money3);
            Assert.False(money1 == money2);
            Assert.False(money1 == money3);
            Assert.True(money1 != money2);
            Assert.True(money1 != money3);
        }

        [Fact]
        public void ValueObject_Equals_Returns_False_For_Null()
        {
            // Arrange
            var money = new Money(100, "USD");
            
            // Act & Assert
            Assert.False(money.Equals(null));
            Assert.False(money == null);
            Assert.False(null == money);
            Assert.True(money != null);
            Assert.True(null != money);
        }

        [Fact]
        public void ValueObject_Equals_Returns_False_For_Different_Types()
        {
            // Arrange
            var money = new Money(100, "USD");
            var differentType = "Not a Money";
            
            // Act & Assert
            Assert.False(money.Equals(differentType));
        }
    }
} 