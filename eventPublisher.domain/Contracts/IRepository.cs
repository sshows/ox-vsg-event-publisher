using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eventPublisher.domain.dataTransferObjects;
using eventPublisher.domain.models;

namespace eventPublisher.domain.contracts
{
    public interface IRepository: IDisposable
    {
        Task<Application> GetApplicationAsync(long applicationId);
        ApplicationEvent GetApplicationEvent(long applicationId, int eventId);
        IEnumerable<string> GetTopics();
        IEnumerable<Subscription> GetSubscriptions(int eventId);
        Subscription GetSubscription(long applicationId, int eventId);
        Task<IEnumerable<ConfigurationDto>> GetConfigurationsAync();
    }
}