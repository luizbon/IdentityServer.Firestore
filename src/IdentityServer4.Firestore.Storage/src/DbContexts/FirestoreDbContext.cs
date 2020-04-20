using System;
using Google.Api.Gax;
using Google.Cloud.Firestore;
using IdentityServer4.Firestore.Storage.Options;

namespace IdentityServer4.Firestore.Storage.DbContexts
{
    public class FirestoreDbContext
    {
        protected FirestoreDbContext(FirestoreOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.ProjectId == null) throw new ArgumentNullException(nameof(options.ProjectId));
            
            Database = new FirestoreDbBuilder
            {
                ProjectId = options.ProjectId,
                EmulatorDetection = EmulatorDetection.EmulatorOrProduction
            }.Build(); 
        }

        protected FirestoreDb Database { get; }
    }
}
