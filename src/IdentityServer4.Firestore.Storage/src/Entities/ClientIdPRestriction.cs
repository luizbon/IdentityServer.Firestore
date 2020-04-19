using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class ClientIdPRestriction
    {
        [FirestoreProperty]
        public string Provider { get; set; }
    }
}