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
        public void Failure_With_Null_Error_Throws_ArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Result.Failure((string)null!));
        }

        [Fact]
        public void Failure_With_Empty_Error_Throws_ArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Result.Failure(string.Empty));
        }

        [Fact]
        public void Failure_With_Whitespace_Error_Throws_ArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Result.Failure("   "));
        }

        [Fact]
        public void Failure_With_Null_Errors_Collection_Throws_ArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Result.Failure((IEnumerable<string>)null!));
        }

        [Fact]
        public void Failure_With_Empty_Errors_Collection_Throws_ArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Result.Failure(new string[0]));
        }

        [Fact]
        public void Failure_With_Params_Array_Creates_Failed_Result()
        {
            // Act
            var result = Result.Failure("Error 1", "Error 2", "Error 3");
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(3, result.Errors.Count);
            Assert.Contains("Error 1", result.Errors);
            Assert.Contains("Error 2", result.Errors);
            Assert.Contains("Error 3", result.Errors);
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

        [Fact]
        public void Generic_Failure_With_Multiple_Errors_Creates_Failed_Result()
        {
            // Arrange
            var errors = new[] { "Error 1", "Error 2" };
            
            // Act
            var result = Result<string>.Failure(errors);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(2, result.Errors.Count);
            Assert.Equal(errors, result.Errors);
            Assert.Throws<InvalidOperationException>(() => result.Value);
        }

        [Fact]
        public void Generic_Failure_With_Null_Error_Throws_ArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Result<int>.Failure((string)null!));
        }

        [Fact]
        public void Generic_Failure_With_Empty_Error_Throws_ArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Result<int>.Failure(string.Empty));
        }

        [Fact]
        public void Generic_Failure_With_Whitespace_Error_Throws_ArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Result<int>.Failure("   "));
        }

        [Fact]
        public void Generic_Failure_With_Null_Errors_Collection_Throws_ArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Result<int>.Failure((IEnumerable<string>)null!));
        }

        [Fact]
        public void Generic_Failure_With_Empty_Errors_Collection_Throws_ArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Result<int>.Failure(new string[0]));
        }

        [Fact]
        public void Generic_Failure_With_Params_Array_Creates_Failed_Result()
        {
            // Act
            var result = Result<string>.Failure("Error 1", "Error 2");
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(2, result.Errors.Count);
            Assert.Contains("Error 1", result.Errors);
            Assert.Contains("Error 2", result.Errors);
        }

        [Fact]
        public void Implicit_Conversion_From_Value_To_Result_Creates_Successful_Result()
        {
            // Arrange
            var value = "test value";
            
            // Act
            Result<string> result = value;
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Errors);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void Implicit_Conversion_With_Null_Value_Creates_Successful_Result()
        {
            // Act
            Result<string?> result = (string?)null;
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Errors);
            Assert.Null(result.Value);
        }

        [Fact]
        public void Value_Property_Throws_When_Result_Is_Failure()
        {
            // Arrange
            var result = Result<int>.Failure("Error");
            
            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => result.Value);
            Assert.Equal("Cannot access the value of a failed result.", exception.Message);
        }
    }
} 