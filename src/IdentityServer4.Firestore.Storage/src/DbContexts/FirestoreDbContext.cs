using System;
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

            Database = FirestoreDb.Create(options.ProjectId);
        }

        protected FirestoreDb Database { get; }
    }
}
