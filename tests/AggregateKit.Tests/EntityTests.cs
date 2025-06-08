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
    }
} 