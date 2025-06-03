# AggregateKit

[![NuGet](https://img.shields.io/nuget/v/AggregateKit.svg)](https://www.nuget.org/packages/AggregateKit)
[![Build](https://github.com/ppilichowski/AggregateKit/actions/workflows/ci.yml/badge.svg)](https://github.com/ppilichowski/AggregateKit/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

AggregateKit is a lightweight library providing essential building blocks for Domain-Driven Design (DDD) in .NET applications.

## Installation

```bash
dotnet add package AggregateKit
```

## Features

- **Entity**: Base class for domain entities with identity-based equality
- **AggregateRoot**: Base class for aggregate roots with domain event handling
- **ValueObject**: Base class for value objects with value-based equality
- **DomainEvents**: Support for domain events through IDomainEvent and DomainEventBase
- **Result**: Functional-style error handling with Result pattern
- **Guard**: Utility methods for validating method arguments and enforcing invariants

## Quick Start

### Value Objects

```csharp
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

// Value objects with the same values are equal
var money1 = new Money(100, "USD");
var money2 = new Money(100, "USD");
Console.WriteLine(money1 == money2); // True
```

### Entities

```csharp
public class Product : Entity<Guid>
{
    public string Name { get; private set; }
    public Money Price { get; private set; }
    
    public Product(Guid id, string name, Money price) : base(id)
    {
        Name = name;
        Price = price;
    }
    
    // Entity behavior methods go here
}

// Entities with the same ID are equal, even if other properties differ
var id = Guid.NewGuid();
var product1 = new Product(id, "Product 1", new Money(100, "USD"));
var product2 = new Product(id, "Updated Name", new Money(110, "USD"));
Console.WriteLine(product1 == product2); // True
```

### Aggregate Roots

```csharp
public class ArticleCreatedEvent : DomainEventBase
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

public class Article : AggregateRoot<Guid>
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public string AuthorName { get; private set; }
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
```

### Result Pattern

```csharp
public Result<Article> CreateArticle(string title, string content, string authorName)
{
    if (string.IsNullOrWhiteSpace(title))
        return Result<Article>.Failure("Title cannot be empty");
        
    if (string.IsNullOrWhiteSpace(authorName))
        return Result<Article>.Failure("Author name cannot be empty");
        
    var article = new Article(Guid.NewGuid(), title, content, authorName);
    return Result<Article>.Success(article);
}

// Usage
var result = CreateArticle("Domain-Driven Design", "DDD concepts explained...", "Eric Evans");
if (result.IsSuccess)
{
    var article = result.Value;
    // Process the article
}
else
{
    // Handle errors
    foreach (var error in result.Errors)
    {
        Console.WriteLine(error);
    }
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 