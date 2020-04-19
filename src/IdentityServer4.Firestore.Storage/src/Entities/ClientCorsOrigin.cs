using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class ClientCorsOrigin
    {
        [FirestoreProperty]
        public string Origin { get; set; }
    }
}