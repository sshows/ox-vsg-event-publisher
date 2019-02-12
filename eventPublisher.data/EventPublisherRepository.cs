using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eventPublisher.data.entities;
using eventPublisher.domain.contracts;
using eventPublisher.domain.models;
using Microsoft.EntityFrameworkCore;

namespace eventPublisher.data {
    public class EventPublisherRepository : IRepository
    {
        private IContext _context;
        public EventPublisherRepository(IContext eventPublisherContext)
        {
            _context = eventPublisherContext ?? throw new ArgumentNullException("EventPublisherContext");
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<Application> GetApplicationAsync(long applicationId)
        {
            ApplicationEntity entity = await _context.Applications.FindAsync(applicationId).ConfigureAwait(false);
            return entity == null ? null : new Application(entity.ApplicationId, entity.Name);
        }

        public ApplicationEvent GetApplicationEvent(long applicationId, int eventId)
        {
            ApplicationEventEntity entity = _context.ApplicationEvents.Include("TopicNav").SingleOrDefault(x => x.EventId == eventId && x.ApplicationId == applicationId);
            return entity == null ? null : new ApplicationEvent(entity.ApplicationId, entity.EventId, entity.TopicNav.Name);
        }

        public IEnumerable<string> GetTopics()
        {
            return _context.Topics.Select(t => t.Name);
        }
    }
}