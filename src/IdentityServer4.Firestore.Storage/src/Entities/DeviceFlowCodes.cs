using System;
using Google.Cloud.Firestore;

namespace IdentityServer4.Firestore.Storage.Entities
{
    [FirestoreData]
    public class DeviceFlowCodes
    {
        [FirestoreProperty]
        public string DeviceCode { get; set; }
        [FirestoreProperty]
        public string UserCode { get; set; }
        [FirestoreProperty]
        public string ClientId { get; set; }
        [FirestoreProperty]
        public string SubjectId { get; set; }
        [FirestoreProperty]
        public DateTime CreationTime { get; set; }
        [FirestoreProperty]
        public DateTime Expiration { get; set; }
        [FirestoreProperty]
        public string Data { get; set; }
    }
}