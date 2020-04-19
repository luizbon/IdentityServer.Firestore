using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class ClientGrantType
    {
        [FirestoreProperty]
        public string GrantType { get; set; }
    }
}