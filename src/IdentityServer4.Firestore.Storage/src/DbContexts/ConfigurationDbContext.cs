using Google.Cloud.Firestore;
using IdentityServer4.Firestore.Storage.Options;

namespace IdentityServer4.Firestore.Storage.DbContexts
{
    public interface IConfigurationDbContext
    {
        CollectionReference Clients { get; }
        CollectionReference ApiResources { get; }
        CollectionReference IdentityResources { get; }
        CollectionReference ApiScopes { get; }
    }

    public class ConfigurationDbContext : FirestoreDbContext, IConfigurationDbContext
    {
        public ConfigurationDbContext(FirestoreOptions options, ConfigurationStoreOptions storeOptions)
            : base(options)
        {
            Schema = Database.Document($"{Constants.IdentityServer}/{storeOptions.Schema}");
            Clients = Schema.Collection(storeOptions.Clients);
            ApiResources = Schema.Collection(storeOptions.ApiResources);
            IdentityResources = Schema.Collection(storeOptions.IdentityResources);
            ApiScopes = Schema.Collection(storeOptions.ApiScopes);
        }

        private DocumentReference Schema { get; }
        public CollectionReference Clients { get; }
        public CollectionReference ApiResources { get; }
        public CollectionReference IdentityResources { get; }
        public CollectionReference ApiScopes { get; }
    }
}
