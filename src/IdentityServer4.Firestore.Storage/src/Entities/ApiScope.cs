using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class ApiScope
    {
        [FirestoreProperty]
        public bool Enabled { get; set; } = true;
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string DisplayName { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public bool Required { get; set; }
        [FirestoreProperty]
        public bool Emphasize { get; set; }
        [FirestoreProperty]
        public bool ShowInDiscoveryDocument { get; set; } = true;
        [FirestoreProperty]
        public List<string> UserClaims { get; set; }
        [FirestoreProperty]
        public Dictionary<string, string> Properties { get; set; }
    }
}
