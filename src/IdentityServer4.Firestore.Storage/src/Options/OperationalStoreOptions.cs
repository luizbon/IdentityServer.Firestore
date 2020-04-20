namespace IdentityServer4.Firestore.Storage.Options
{
    public class OperationalStoreOptions
    {
        public string Schema { get; set; } = Constants.OperationalStore;
        public string DeviceFlowCodes { get; set; } = Constants.DeviceFlowCodes;
        public string PersistedGrants { get; set; } = Constants.PersistedGrants;
        public bool EnableTokenCleanup { get; set; } = false;
        public int TokenCleanupInterval { get; set; } = 3600;
        public int TokenCleanupBatchSize { get; set; } = 100;
    }
}
