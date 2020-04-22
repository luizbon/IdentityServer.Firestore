using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Firestore.Storage.Entities;

namespace IdentityServer4.Firestore.Storage.TokenCleanup
{
    public interface IOperationalStoreNotification
    {
        Task PersistedGrantsRemovedAsync(IEnumerable<PersistedGrant> persistedGrants);
        Task DeviceCodesRemovedAsync(IEnumerable<DeviceFlowCodes> deviceCodes);
    }
}