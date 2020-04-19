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
            Clients = Database.Collection($"{storeOptions.Clients.Parent}/{storeOptions.Clients.Name}");
            ApiResources = Database.Collection($"{storeOptions.ApiResources.Parent}/{storeOptions.ApiResources.Name}");
            IdentityResources = Database.Collection($"{storeOptions.IdentityResources.Parent}/{storeOptions.IdentityResources.Name}");
            ApiScopes = Database.Collection($"{storeOptions.ApiScopes.Parent}/{storeOptions.ApiScopes.Name}");
        }

        public CollectionReference Clients { get; }
        public CollectionReference ApiResources { get; }
        public CollectionReference IdentityResources { get; }
        public CollectionReference ApiScopes { get; }
    }
}
