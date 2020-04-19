using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class ClientScope
    {
        [FirestoreProperty]
        public string Scope { get; set; }
    }
}