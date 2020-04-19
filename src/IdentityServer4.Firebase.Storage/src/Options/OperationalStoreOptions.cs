namespace IdentityServer4.Firestore.Storage.Options
{
    public class OperationalStoreOptions
    {
        public CollectionConfiguration DeviceFlowCodes { get; set; } = new CollectionConfiguration(Constants.DeviceFlowCodes, Constants.IdentityServer);
        public CollectionConfiguration PersistedGrants { get; set; } = new CollectionConfiguration(Constants.PersistedGrants, Constants.IdentityServer);
        public bool EnableTokenCleanup { get; set; } = false;
        public int TokenCleanupInterval { get; set; } = 3600;
        public int TokenCleanupBatchSize { get; set; } = 100;
    }
}
