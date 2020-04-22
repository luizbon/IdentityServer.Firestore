using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using IdentityServer4.Firestore.Storage.DbContexts;
using IdentityServer4.Firestore.Storage.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Firestore.Storage.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IPersistedGrantDbContext _context;
        private readonly ILogger<DeviceFlowStore> _logger;

        public PersistedGrantStore(IPersistedGrantDbContext context, ILogger<DeviceFlowStore> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            DocumentSnapshot persistedGrant =
                await GetPersistedGrant(nameof(PersistedGrant.Key), grant.Key).ConfigureAwait(false);
            if (persistedGrant.Exists)
            {
                _logger.LogDebug("{persistedGrantKey} found in database", grant.Key);

                Entities.PersistedGrant existing = persistedGrant.ConvertTo<Entities.PersistedGrant>();

                grant.UpdateEntity(existing);

                await persistedGrant.Reference.SetAsync(existing).ConfigureAwait(false);
            }
            else
            {
                _logger.LogDebug("{persistedGrant} not found in database", grant.Key);

                Entities.PersistedGrant persistedGrand = grant.ToEntity();
                await _context.PersistedGrants.AddAsync(persistedGrand).ConfigureAwait(false);
            }
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            DocumentSnapshot persistedGrant =
                await GetPersistedGrant(nameof(PersistedGrant.Key), key).ConfigureAwait(false);

            _logger.LogDebug("{persistedGrandKey} found in database: {persistedGrantKeyFound}", key,
                persistedGrant.Exists);

            return persistedGrant.ConvertTo<PersistedGrant>();
        }

        public virtual async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            QuerySnapshot persistedGrants = await GetPersistedGrants((nameof(PersistedGrant.SubjectId), subjectId))
                .ConfigureAwait(false);

            List<PersistedGrant> model = persistedGrants
                .Where(persistedGrant => persistedGrant.Exists)
                .Select(persistedGrant => persistedGrant.ConvertTo<PersistedGrant>())
                .ToList();

            _logger.LogDebug("{persistedGrantCount} persisted grants found for {subjectId}", model.Count, subjectId);

            return model;
        }

        public async Task RemoveAsync(string key)
        {
            DocumentSnapshot persistedGrant =
                await GetPersistedGrant(nameof(PersistedGrant.Key), key).ConfigureAwait(false);
            if (persistedGrant.Exists)
            {
                _logger.LogDebug("removing {persistedGrantKey} persisted grant from database", key);

                await persistedGrant.Reference.DeleteAsync(Precondition.None).ConfigureAwait(false);
            }
            else
            {
                _logger.LogDebug("no {persistedGrantKey} persisted grant found in database", key);
            }
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            QuerySnapshot persistedGrants = await GetPersistedGrants(
                    (nameof(PersistedGrant.SubjectId), subjectId),
                    (nameof(PersistedGrant.ClientId), clientId))
                .ConfigureAwait(false);

            _logger.LogDebug(
                "removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}",
                persistedGrants.Count, subjectId, clientId);

            foreach (DocumentSnapshot persistedGrant in persistedGrants)
            {
                await persistedGrant.Reference.DeleteAsync(Precondition.None).ConfigureAwait(false);
            }
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            QuerySnapshot persistedGrants = await GetPersistedGrants(
                    (nameof(PersistedGrant.SubjectId), subjectId),
                    (nameof(PersistedGrant.ClientId), clientId),
                    (nameof(PersistedGrant.Type), type))
                .ConfigureAwait(false);

            _logger.LogDebug(
                "removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {persistedGrantType}",
                persistedGrants.Count, subjectId, clientId, type);

            foreach (DocumentSnapshot persistedGrant in persistedGrants)
            {
                await persistedGrant.Reference.DeleteAsync(Precondition.None).ConfigureAwait(false);
            }
        }

        private async Task<QuerySnapshot> GetPersistedGrants(params (string property, object value)[] filters)
        {
            Query query = _context.PersistedGrants.OrderBy(FieldPath.DocumentId);
            query = filters.Aggregate(query, (current, filter) => current.WhereEqualTo(filter.property, filter.value));

            return await query.GetSnapshotAsync().ConfigureAwait(false);
        }

        private async Task<DocumentSnapshot> GetPersistedGrant(string property, object value)
        {
            return (await _context.PersistedGrants.WhereEqualTo(property, value)
                .Limit(1).GetSnapshotAsync().ConfigureAwait(false)).First();
        }
    }
}