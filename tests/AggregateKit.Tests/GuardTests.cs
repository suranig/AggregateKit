using System;
using System.Collections.Generic;
using Xunit;

namespace AggregateKit.Tests
{
    public class GuardTests
    {
        [Fact]
        public void AgainstNull_Throws_When_Value_Is_Null()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Guard.AgainstNull(null, "param"));
        }

        [Fact]
        public void AgainstNull_Does_Not_Throw_When_Value_Is_Not_Null()
        {
            // Act
            Guard.AgainstNull("not null", "param");
            
            // Assert
            // No exception means the test passes
        }

        [Fact]
        public void AgainstNullOrEmpty_Throws_When_String_Is_Null()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Guard.AgainstNullOrEmpty(null, "param"));
        }

        [Fact]
        public void AgainstNullOrEmpty_Throws_When_String_Is_Empty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Guard.AgainstNullOrEmpty(string.Empty, "param"));
        }

        [Fact]
        public void AgainstNullOrEmpty_Does_Not_Throw_When_String_Is_Not_Empty()
        {
            // Act
            Guard.AgainstNullOrEmpty("not empty", "param");
            
            // Assert
            // No exception means the test passes
        }

        [Fact]
        public void AgainstNullOrWhiteSpace_Throws_When_String_Is_Null()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Guard.AgainstNullOrWhiteSpace(null, "param"));
        }

        [Fact]
        public void AgainstNullOrWhiteSpace_Throws_When_String_Is_Empty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Guard.AgainstNullOrWhiteSpace(string.Empty, "param"));
        }

        [Fact]
        public void AgainstNullOrWhiteSpace_Throws_When_String_Is_WhiteSpace()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Guard.AgainstNullOrWhiteSpace("   ", "param"));
        }

        [Fact]
        public void AgainstNullOrWhiteSpace_Does_Not_Throw_When_String_Is_Not_WhiteSpace()
        {
            // Act
            Guard.AgainstNullOrWhiteSpace("not whitespace", "param");
            
            // Assert
            // No exception means the test passes
        }

        [Fact]
        public void AgainstNullOrEmpty_Collection_Throws_When_Collection_Is_Null()
        {
            // Act & Assert
            IEnumerable<int>? nullCollection = null;
            Assert.Throws<ArgumentNullException>(() => Guard.AgainstNullOrEmpty(nullCollection, "param"));
        }

        [Fact]
        public void AgainstNullOrEmpty_Collection_Throws_When_Collection_Is_Empty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Guard.AgainstNullOrEmpty(new List<int>(), "param"));
        }

        [Fact]
        public void AgainstNullOrEmpty_Collection_Does_Not_Throw_When_Collection_Is_Not_Empty()
        {
            // Act
            Guard.AgainstNullOrEmpty(new[] { 1, 2, 3 }, "param");
            
            // Assert
            // No exception means the test passes
        }

        [Fact]
        public void AgainstCondition_Throws_When_Condition_Is_True()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Guard.AgainstCondition(true, "Error message", "param"));
        }

        [Fact]
        public void AgainstCondition_Does_Not_Throw_When_Condition_Is_False()
        {
            // Act
            Guard.AgainstCondition(false, "Error message", "param");
            
            // Assert
            // No exception means the test passes
        }
    }
} 