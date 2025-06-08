using System;
using System.Linq;
using Xunit;

namespace AggregateKit.Tests
{
    public class AggregateRootTests
    {
        private class ArticleCreatedEvent : DomainEventBase
        {
            public Guid ArticleId { get; }
            public string Title { get; }
            public string AuthorName { get; }

            public ArticleCreatedEvent(Guid articleId, string title, string authorName)
            {
                ArticleId = articleId;
                Title = title;
                AuthorName = authorName;
            }
        }

        private class ArticlePublishedEvent : DomainEventBase
        {
            public Guid ArticleId { get; }
            public DateTime PublishDate { get; }

            public ArticlePublishedEvent(Guid articleId, DateTime publishDate)
            {
                ArticleId = articleId;
                PublishDate = publishDate;
            }
        }

        private class TestEvent : DomainEventBase
        {
        }

        private class TestAggregate : AggregateRoot<Guid>
        {
            public TestAggregate(Guid id) : base(id)
            {
                AddDomainEvent(new TestEvent());
            }
        }

        private class Article : AggregateRoot<Guid>
        {
            public string Title { get; private set; } = string.Empty;
            public string Content { get; private set; } = string.Empty;
            public string AuthorName { get; private set; } = string.Empty;
            public bool IsPublished { get; private set; }
            public DateTime? PublishDate { get; private set; }

            private Article() { }

            public Article(Guid id, string title, string content, string authorName) : base(id)
            {
                Title = title;
                Content = content;
                AuthorName = authorName;
                IsPublished = false;
                PublishDate = null;
                
                AddDomainEvent(new ArticleCreatedEvent(id, title, authorName));
            }

            public void Publish()
            {
                if (IsPublished)
                    return;
                
                IsPublished = true;
                PublishDate = DateTime.UtcNow;
                
                AddDomainEvent(new ArticlePublishedEvent(Id, PublishDate.Value));
            }
        }

        [Fact]
        public void AggregateRoot_Adds_Domain_Events()
        {
            // Arrange
            var articleId = Guid.NewGuid();
            var title = "Domain-Driven Design Patterns";
            var content = "Content of the article about DDD patterns";
            var authorName = "Jane Smith";
            
            // Act
            var article = new Article(articleId, title, content, authorName);
            article.Publish();
            
            // Assert
            Assert.Equal(2, article.DomainEvents.Count);
            
            var articleCreatedEvent = article.DomainEvents.First() as ArticleCreatedEvent;
            Assert.NotNull(articleCreatedEvent);
            Assert.Equal(articleId, articleCreatedEvent.ArticleId);
            Assert.Equal(title, articleCreatedEvent.Title);
            Assert.Equal(authorName, articleCreatedEvent.AuthorName);
            
            var articlePublishedEvent = article.DomainEvents.Last() as ArticlePublishedEvent;
            Assert.NotNull(articlePublishedEvent);
            Assert.Equal(articleId, articlePublishedEvent.ArticleId);
            Assert.True(articlePublishedEvent.PublishDate <= DateTime.UtcNow);
        }

        [Fact]
        public void ClearDomainEvents_Removes_All_Events()
        {
            // Arrange
            var article = new Article(
                Guid.NewGuid(), 
                "Clean Architecture", 
                "Content about clean architecture principles", 
                "Robert Martin"
            );
            article.Publish();
            
            // Act
            article.ClearDomainEvents();
            
            // Assert
            Assert.Empty(article.DomainEvents);
        }

        [Fact]
        public void DomainEvents_IsReadOnly()
        {
            // Arrange
            var aggregate = new TestAggregate(Guid.NewGuid());

            // Act
            var events = aggregate.DomainEvents;
            var collection = (ICollection<IDomainEvent>)events;

            // Assert
            Assert.Throws<NotSupportedException>(() => collection.Add(new TestEvent()));
            Assert.Throws<NotSupportedException>(() => collection.Clear());
        }
    }
}
