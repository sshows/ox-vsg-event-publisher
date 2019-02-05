using System;
using System.Threading.Tasks;
using eventPublisher.domain.models;

namespace eventPublisher.domain.contracts
{
    public interface IRepository: IDisposable
    {
        Task<Application> GetApplicationAsync(long applicationId);
        ApplicationEvent GetApplicationEvent(long applicationId, int eventId);
    }
}