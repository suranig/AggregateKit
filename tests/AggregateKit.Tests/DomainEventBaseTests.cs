using System;
using Xunit;

namespace AggregateKit.Tests
{
    public class DomainEventBaseTests
    {
        private class TestEvent : DomainEventBase
        {
        }

        [Fact]
        public void DomainEventBase_Assigns_NonEmpty_Id()
        {
            var evt = new TestEvent();

            Assert.NotEqual(Guid.Empty, evt.Id);
        }

        [Fact]
        public void DomainEventBase_OccurredOn_Is_Close_To_UtcNow()
        {
            var evt = new TestEvent();

            var timeDiff = DateTime.UtcNow - evt.OccurredOn;

            Assert.True(timeDiff < TimeSpan.FromSeconds(2), $"OccurredOn difference was {timeDiff.TotalSeconds}");
        }

        [Fact]
        public void DomainEventBase_Generates_Unique_Ids()
        {
            var evt1 = new TestEvent();
            var evt2 = new TestEvent();

            Assert.NotEqual(evt1.Id, evt2.Id);
        }
    }
}
