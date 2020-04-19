using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class UserClaim
    {
        [FirestoreProperty]
        public string Type { get; set; }
    }
}