# AggregateKit

AggregateKit is a lightweight library providing essential building blocks for Domain-Driven Design (DDD) in .NET applications.

## Features

- **Entity**: Base class for domain entities with identity-based equality
- **AggregateRoot**: Base class for aggregate roots with domain event handling
- **ValueObject**: Base class for value objects with value-based equality
- **DomainEvents**: Support for domain events through IDomainEvent and DomainEventBase
- **Result**: Functional-style error handling with Result pattern
- **Guard**: Utility methods for validating method arguments and enforcing invariants

## Quick Start

```csharp
// Create a value object
public class Money : ValueObject
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

// Create an entity
public class Product : Entity<Guid>
{
    public string Name { get; private set; }
    public Money Price { get; private set; }
    
    public Product(Guid id, string name, Money price) : base(id)
    {
        Name = name;
        Price = price;
    }
}

// Create an aggregate root
public class Article : AggregateRoot<Guid>
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public string AuthorName { get; private set; }
    
    public Article(Guid id, string title, string content, string authorName) : base(id)
    {
        Title = title;
        Content = content;
        AuthorName = authorName;
        
        AddDomainEvent(new ArticleCreatedEvent(id, title, authorName));
    }
}
``` 