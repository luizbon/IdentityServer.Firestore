using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class IdentityResource
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
        public List<UserClaim> UserClaims { get; set; }
        [FirestoreProperty]
        public List<Property> Properties { get; set; }
        [FirestoreProperty]
        public DateTime Created { get; set; } = DateTime.UtcNow;
        [FirestoreProperty]
        public DateTime? Updated { get; set; }
        [FirestoreProperty]
        public bool NonEditable { get; set; }
    }
}
