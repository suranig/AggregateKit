using System;
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

        private class Address : ValueObject
        {
            public string Street { get; }
            public string City { get; }
            public string? PostalCode { get; }
            
            public Address(string street, string city, string? postalCode = null)
            {
                Street = street;
                City = city;
                PostalCode = postalCode;
            }
            
            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return Street;
                yield return City;
                yield return PostalCode;
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

        [Fact]
        public void ValueObject_IEquatable_Equals_Returns_True_For_Same_Values()
        {
            // Arrange
            var money1 = new Money(100, "USD");
            var money2 = new Money(100, "USD");
            
            // Act & Assert
            Assert.True(((IEquatable<ValueObject>)money1).Equals(money2));
        }

        [Fact]
        public void ValueObject_IEquatable_Equals_Returns_False_For_Null()
        {
            // Arrange
            var money = new Money(100, "USD");
            
            // Act & Assert
            Assert.False(((IEquatable<ValueObject>)money).Equals(null));
        }

        [Fact]
        public void ValueObject_IEquatable_Equals_Returns_False_For_Different_Types()
        {
            // Arrange
            var money = new Money(100, "USD");
            var address = new Address("123 Main St", "Anytown");
            
            // Act & Assert
            Assert.False(((IEquatable<ValueObject>)money).Equals(address));
        }

        [Fact]
        public void ValueObject_IEquatable_Equals_Returns_True_For_Same_Reference()
        {
            // Arrange
            var money = new Money(100, "USD");
            
            // Act & Assert
            Assert.True(((IEquatable<ValueObject>)money).Equals(money));
        }

        [Fact]
        public void ValueObject_Operators_Handle_Both_Null()
        {
            // Arrange
            Money? money1 = null;
            Money? money2 = null;
            
            // Act & Assert
            Assert.True(money1 == money2);
            Assert.False(money1 != money2);
        }

        [Fact]
        public void ValueObject_Operators_Handle_One_Null()
        {
            // Arrange
            var money = new Money(100, "USD");
            Money? nullMoney = null;
            
            // Act & Assert
            Assert.False(money == nullMoney);
            Assert.False(nullMoney == money);
            Assert.True(money != nullMoney);
            Assert.True(nullMoney != money);
        }

        [Fact]
        public void ValueObject_With_Null_Components_Are_Equal()
        {
            // Arrange
            var address1 = new Address("123 Main St", "Anytown", null);
            var address2 = new Address("123 Main St", "Anytown", null);
            
            // Act & Assert
            Assert.Equal(address1, address2);
            Assert.True(address1 == address2);
            Assert.False(address1 != address2);
            Assert.Equal(address1.GetHashCode(), address2.GetHashCode());
        }

        [Fact]
        public void ValueObject_With_Different_Null_Components_Are_Not_Equal()
        {
            // Arrange
            var address1 = new Address("123 Main St", "Anytown", null);
            var address2 = new Address("123 Main St", "Anytown", "12345");
            
            // Act & Assert
            Assert.NotEqual(address1, address2);
            Assert.False(address1 == address2);
            Assert.True(address1 != address2);
        }

        [Fact]
        public void ValueObject_HashCode_Is_Consistent()
        {
            // Arrange
            var money = new Money(100, "USD");
            var expectedHashCode = money.GetHashCode();
            
            // Act & Assert
            // Hash code should be consistent across multiple calls
            Assert.Equal(expectedHashCode, money.GetHashCode());
            Assert.Equal(expectedHashCode, money.GetHashCode());
        }

        [Fact]
        public void ValueObject_HashCode_Handles_Null_Components()
        {
            // Arrange
            var address = new Address("123 Main St", "Anytown", null);
            
            // Act & Assert
            // Should not throw exception when calculating hash code with null components
            var hashCode = address.GetHashCode();
            Assert.True(hashCode != 0 || hashCode == 0); // Just ensure it doesn't throw
        }
    }
} 