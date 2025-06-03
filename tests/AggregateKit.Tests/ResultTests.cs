using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AggregateKit.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Success_Creates_Successful_Result()
        {
            // Act
            var result = Result.Success();
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Failure_With_Single_Error_Creates_Failed_Result()
        {
            // Arrange
            var errorMessage = "Something went wrong";
            
            // Act
            var result = Result.Failure(errorMessage);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Single(result.Errors);
            Assert.Equal(errorMessage, result.Errors.First());
        }

        [Fact]
        public void Failure_With_Multiple_Errors_Creates_Failed_Result()
        {
            // Arrange
            var errors = new[] { "Error 1", "Error 2", "Error 3" };
            
            // Act
            var result = Result.Failure(errors);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(3, result.Errors.Count);
            Assert.Equal(errors, result.Errors);
        }

        [Fact]
        public void Generic_Success_Creates_Successful_Result_With_Value()
        {
            // Arrange
            var value = 42;
            
            // Act
            var result = Result<int>.Success(value);
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Errors);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void Generic_Failure_Creates_Failed_Result()
        {
            // Arrange
            var errorMessage = "Something went wrong";
            
            // Act
            var result = Result<int>.Failure(errorMessage);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Single(result.Errors);
            Assert.Equal(errorMessage, result.Errors.First());
            Assert.Throws<InvalidOperationException>(() => result.Value);
        }
    }
} 