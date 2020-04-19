using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public abstract class Property
    {
        [FirestoreProperty]
        public string Key { get; set; }
        [FirestoreProperty]
        public string Value { get; set; }
    }
}