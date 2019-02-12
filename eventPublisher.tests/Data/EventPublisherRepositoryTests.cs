using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eventPublisher.data;
using eventPublisher.data.entities;
using eventPublisher.domain.models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eventPublisher.tests.data
{
    [Trait("Category", "Unit")]
    public class EventPublisherRepositoryTests
    {
        [Fact]
        public void EventPublisherRepository_ctor_requiresContext()
        {
            // act & asert
            Assert.Throws<ArgumentNullException>(() => new EventPublisherRepository((EventPublisherContext)null));
        }

        [Fact]
        public async Task EventPublisherRepository_GetApplicationAsync_ReturnsNullWhenNotFound()
        {
            // arrange
            EventPublisherContext context = GetContext();
            var target = new EventPublisherRepository(context);

            // act
            Application result = await target.GetApplicationAsync(1);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task EventPublisherRepository_GetApplicationAsync_ReturnsApplicationWhenFound()
        {
            // arrange
            EventPublisherContext context = GetContext();
            var applicationId = context.Applications.Add(new ApplicationEntity
            {

            }).Entity.ApplicationId;
            await context.SaveChangesAsync().ConfigureAwait(false);
            var target = new EventPublisherRepository(context);

            // act
            Application result = await target.GetApplicationAsync(applicationId);

            // assert
            Assert.NotNull(result);
        }

        [Fact]
        public void EventPublisherRepository_GetApplicationEvent_ReturnsNullWhenNotFound()
        {
            // arrange
            EventPublisherContext context = GetContext();
            var target = new EventPublisherRepository(context);

            // act
            ApplicationEvent result = target.GetApplicationEvent(1, 1);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task EventPublisherRepository_GetApplicationEvent_ReturnsNullIfEventDoesNotBelongToApplication()
        {
            // arrange
            EventPublisherContext context = GetContext();
            var applicationId = context.Applications.Add(new ApplicationEntity
            {

            }).Entity.ApplicationId;
            var eventId = context.ApplicationEvents.Add(new ApplicationEventEntity
            {
                ApplicationId = applicationId + 1
            }).Entity.EventId;
            await context.SaveChangesAsync().ConfigureAwait(false);
            var target = new EventPublisherRepository(context);

            // act
            ApplicationEvent result = target.GetApplicationEvent(applicationId, eventId);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task EventPublisherRepository_GetApplicationEvent_ReturnsApplicationEventWhenFound()
        {
            // arrange
            EventPublisherContext context = GetContext();
            var applicationId = context.Applications.Add(new ApplicationEntity
            {

            }).Entity.ApplicationId;
            var topicId = context.Topics.Add(new TopicEntity
            {
                Name = "Test",
            }).Entity.TopicId;
            var eventId = context.ApplicationEvents.Add(new ApplicationEventEntity
            {
                ApplicationId = applicationId,
                Name = "Test",
                TopicId = topicId
            }).Entity.EventId;
            await context.SaveChangesAsync().ConfigureAwait(false);
            var target = new EventPublisherRepository(context);

            // act
            ApplicationEvent result = target.GetApplicationEvent(applicationId, eventId);

            // assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EventPublisherRepository_GetTopics_ReturnsTopicNames()
        {
            // arrange
            EventPublisherContext context = GetContext();
            context.Topics.Add(new TopicEntity
            {
                Name = "Test",
            });
            context.Topics.Add(new TopicEntity
            {
                Name = "Test2",
            });
            context.Topics.Add(new TopicEntity
            {
                Name = "Test3",
            });
            await context.SaveChangesAsync().ConfigureAwait(false);
            var target = new EventPublisherRepository(context);

            // act
            IEnumerable<string> result = target.GetTopics();

            // assert
            Assert.NotNull(result);
        }

        private EventPublisherContext GetContext()
        {
            // NOTE: Keys in DB do not automatically reset with new in memory db, see:  https://github.com/aspnet/EntityFrameworkCore/issues/6872
            return new EventPublisherContext(new DbContextOptionsBuilder<EventPublisherContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        }
    }

}