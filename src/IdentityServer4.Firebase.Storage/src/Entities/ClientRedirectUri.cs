using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class ClientRedirectUri
    {
        [FirestoreProperty]
        public string RedirectUri { get; set; }
    }
}