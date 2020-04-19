namespace IdentityServer4.Firestore.Storage.Options
{
    public class ConfigurationStoreOptions
    {
        public CollectionConfiguration Clients { get; set; } = new CollectionConfiguration(Constants.Clients, Constants.IdentityServer);
        public CollectionConfiguration ApiResources { get; set; } = new CollectionConfiguration(Constants.ApiResources, Constants.IdentityServer);
        public CollectionConfiguration IdentityResources { get; set; } = new CollectionConfiguration(Constants.IdentityResources, Constants.IdentityServer);
        public CollectionConfiguration ApiScopes { get; set; } = new CollectionConfiguration(Constants.ApiScopes, Constants.IdentityServer);
    }
}
