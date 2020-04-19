using Google.Cloud.Firestore;
using IdentityServer4.Firestore.Storage.Options;

namespace IdentityServer4.Firestore.Storage.DbContexts
{
    public interface IPersistedGrantDbContext
    {
        CollectionReference PersistedGrants { get; }
        CollectionReference DeviceFlowCodes { get; }
    }

    public class PersistedGrantDbContext: FirestoreDbContext, IPersistedGrantDbContext
    {
        protected PersistedGrantDbContext(FirestoreOptions options, OperationalStoreOptions storeOptions) : base(options)
        {
            PersistedGrants = Database.Collection($"{storeOptions.PersistedGrants.Parent}/{storeOptions.PersistedGrants.Name}");
            DeviceFlowCodes = Database.Collection($"{storeOptions.DeviceFlowCodes.Parent}/{storeOptions.DeviceFlowCodes.Name}");
        }

        public CollectionReference PersistedGrants { get; }
        public CollectionReference DeviceFlowCodes { get; }
    }
}
