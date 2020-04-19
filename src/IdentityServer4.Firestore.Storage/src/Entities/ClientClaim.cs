using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class ClientClaim
    {
        [FirestoreProperty]
        public string Type { get; set; }
        [FirestoreProperty]
        public string Value { get; set; }
    }
}