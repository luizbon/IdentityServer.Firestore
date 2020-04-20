using Google.Cloud.Firestore;
using IdentityServer4.Firestore.Storage.Options;

namespace IdentityServer4.Firestore.Storage.DbContexts
{
    public interface IPersistedGrantDbContext
    {
        CollectionReference PersistedGrants { get; }
        CollectionReference DeviceFlowCodes { get; }
    }

    public class PersistedGrantDbContext : FirestoreDbContext, IPersistedGrantDbContext
    {
        public PersistedGrantDbContext(FirestoreOptions options, OperationalStoreOptions storeOptions)
            : base(options)
        {
            Schema = Database.Document($"{Constants.IdentityServer}/{storeOptions.Schema}");

            PersistedGrants = Schema.Collection(storeOptions.PersistedGrants);
            DeviceFlowCodes = Schema.Collection(storeOptions.DeviceFlowCodes);
        }

        private DocumentReference Schema { get; }
        public CollectionReference PersistedGrants { get; }
        public CollectionReference DeviceFlowCodes { get; }
    }
}
