using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class ClientPostLogoutRedirectUri
    {
        [FirestoreProperty]
        public string PostLogoutRedirectUri { get; set; }
    }
}