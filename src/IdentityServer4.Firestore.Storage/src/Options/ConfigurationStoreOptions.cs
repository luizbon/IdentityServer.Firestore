namespace IdentityServer4.Firestore.Storage.Options
{
    public class ConfigurationStoreOptions
    {
        public string Schema { get; set; } = Constants.ConfigurationStore;
        public string Clients { get; set; } = Constants.Clients;
        public string ApiResources { get; set; } = Constants.ApiResources;
        public string IdentityResources { get; set; } = Constants.IdentityResources;
        public string ApiScopes { get; set; } = Constants.ApiScopes;
    }
}