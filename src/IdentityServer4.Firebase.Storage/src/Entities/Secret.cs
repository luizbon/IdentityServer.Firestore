using System;
using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class Secret
    {
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string Value { get; set; }
        [FirestoreProperty]
        public DateTime? Expiration { get; set; }
        [FirestoreProperty]
        public string Type { get; set; } = "SharedSecret";
        [FirestoreProperty]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}