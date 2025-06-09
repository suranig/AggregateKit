using System;
using Xunit;

namespace AggregateKit.Tests
{
    public class EntityTests
    {
        private class TestEntity : Entity<Guid>
        {
            public string Name { get; private set; }

            public TestEntity(Guid id, string name) : base(id)
            {
                Name = name;
            }

            // For EF Core
            private TestEntity() : base() 
            {
                Name = string.Empty;
            }
        }

        private class StringEntity : Entity<string>
        {
            public string Name { get; private set; }

            public StringEntity(string id, string name) : base(id)
            {
                Name = name;
            }

            // For EF Core
            private StringEntity() : base() 
            {
                Name = string.Empty;
            }
        }

        private class DifferentEntity : Entity<Guid>
        {
            public DifferentEntity(Guid id) : base(id) { }
        }

        [Fact]
        public void Entity_With_Same_Id_Is_Equal()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id, "Entity 1");
            var entity2 = new TestEntity(id, "Entity 2"); // Different name, same ID
            
            // Act & Assert
            Assert.Equal(entity1, entity2);
            Assert.True(entity1 == entity2);
            Assert.False(entity1 != entity2);
            Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
        }

        [Fact]
        public void Entity_Self_Equality()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid(), "Entity");
            var sameReference = entity; // two variables referencing the same object

            // Act & Assert
            Assert.True(entity == sameReference);
            Assert.False(entity != sameReference);
            Assert.True(entity.Equals(sameReference));
        }

        [Fact]
        public void Entity_With_Different_Id_Is_Not_Equal()
        {
            // Arrange
            var entity1 = new TestEntity(Guid.NewGuid(), "Entity 1");
            var entity2 = new TestEntity(Guid.NewGuid(), "Entity 1"); // Same name, different ID
            
            // Act & Assert
            Assert.NotEqual(entity1, entity2);
            Assert.False(entity1 == entity2);
            Assert.True(entity1 != entity2);
            Assert.NotEqual(entity1.GetHashCode(), entity2.GetHashCode());
        }

        [Fact]
        public void Entity_Equals_Returns_False_For_Null()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid(), "Entity");
            
            // Act & Assert
            Assert.False(entity.Equals(null));
            Assert.False(entity == null);
            Assert.False(null == entity);
            Assert.True(entity != null);
            Assert.True(null != entity);
        }

        [Fact]
        public void Entity_Equals_Returns_False_For_Different_Types()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid(), "Entity");
            var differentType = "Not an Entity";
            
            // Act & Assert
            Assert.False(entity.Equals(differentType));
        }

        [Fact]
        public void Entity_Cannot_Be_Created_With_Default_Id()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TestEntity(Guid.Empty, "Entity"));
        }

        [Fact]
        public void Entity_Constructor_Validates_Id_Parameter()
        {
            // This test verifies that the constructor properly validates the id parameter
            // Since Guid is a value type, we can't pass null directly, but we can test with default
            // The ArgumentNullException.ThrowIfNull will be called for reference types
            
            // Act & Assert - Test with default value which should throw ArgumentException
            Assert.Throws<ArgumentException>(() => new TestEntity(default(Guid), "Entity"));
        }

        [Fact]
        public void Entity_With_Reference_Type_Id_Cannot_Be_Created_With_Null()
        {
            // Act & Assert - This tests the ArgumentNullException.ThrowIfNull path
            Assert.Throws<ArgumentNullException>(() => new StringEntity(null!, "Entity"));
        }

        [Fact]
        public void Entity_With_Reference_Type_Id_Can_Be_Created_With_Valid_String()
        {
            // Act & Assert - This tests that valid string IDs work correctly
            var entity = new StringEntity("valid-id", "Entity");
            Assert.Equal("valid-id", entity.Id);
            Assert.Equal("Entity", entity.Name);
        }

        [Fact]
        public void Entity_IEquatable_Equals_Returns_True_For_Same_Id()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id, "Entity 1");
            var entity2 = new TestEntity(id, "Entity 2");
            
            // Act & Assert
            Assert.True(((IEquatable<Entity<Guid>>)entity1).Equals(entity2));
        }

        [Fact]
        public void Entity_IEquatable_Equals_Returns_False_For_Null()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid(), "Entity");
            
            // Act & Assert
            Assert.False(((IEquatable<Entity<Guid>>)entity).Equals(null));
        }

        [Fact]
        public void Entity_IEquatable_Equals_Returns_False_For_Different_Types()
        {
            // Arrange
            var id = Guid.NewGuid();
            var testEntity = new TestEntity(id, "Entity");
            var differentEntity = new DifferentEntity(id);
            
            // Act & Assert
            Assert.False(((IEquatable<Entity<Guid>>)testEntity).Equals(differentEntity));
        }

        [Fact]
        public void Entity_IEquatable_Equals_Returns_True_For_Same_Reference()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid(), "Entity");
            
            // Act & Assert
            Assert.True(((IEquatable<Entity<Guid>>)entity).Equals(entity));
        }

        [Fact]
        public void Entity_Operators_Handle_Both_Null()
        {
            // Arrange
            TestEntity? entity1 = null;
            TestEntity? entity2 = null;
            
            // Act & Assert
            Assert.True(entity1 == entity2);
            Assert.False(entity1 != entity2);
        }

        [Fact]
        public void Entity_Operators_Handle_One_Null()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid(), "Entity");
            TestEntity? nullEntity = null;
            
            // Act & Assert
            Assert.False(entity == nullEntity);
            Assert.False(nullEntity == entity);
            Assert.True(entity != nullEntity);
            Assert.True(nullEntity != entity);
        }

        [Fact]
        public void Entity_GetHashCode_Uses_Id_HashCode()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new TestEntity(id, "Entity");
            
            // Act & Assert
            Assert.Equal(id.GetHashCode(), entity.GetHashCode());
        }

        [Fact]
        public void StringEntity_With_Same_Id_Are_Equal()
        {
            // Arrange
            var id = "test-id";
            var entity1 = new StringEntity(id, "Entity 1");
            var entity2 = new StringEntity(id, "Entity 2");
            
            // Act & Assert
            Assert.Equal(entity1, entity2);
            Assert.True(entity1 == entity2);
            Assert.False(entity1 != entity2);
            Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
        }

        [Fact]
        public void Entity_Parameterless_Constructor_Works_For_EF_Core()
        {
            // This test ensures the parameterless constructor works (needed for EF Core)
            // We can't directly test it since it's protected, but we can verify it exists
            // by checking that the type can be instantiated through reflection

            var entityType = typeof(TestEntity);
            var constructors = entityType.GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var parameterlessConstructor = Array.Find(constructors, c => c.GetParameters().Length == 0);

            Assert.NotNull(parameterlessConstructor);
        }

        [Fact]
        public void Entity_ParameterlessConstructor_CanInstantiate()
        {
            var entity = new ParameterlessEntity();

            Assert.NotNull(entity);
            Assert.Equal(default(Guid), entity.Id);
        }
    }
}
