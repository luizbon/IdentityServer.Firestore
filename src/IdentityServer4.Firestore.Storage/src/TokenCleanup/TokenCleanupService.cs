using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using IdentityServer4.Firestore.Storage.DbContexts;
using IdentityServer4.Firestore.Storage.Entities;
using IdentityServer4.Firestore.Storage.Options;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Firestore.Storage.TokenCleanup
{
    public class TokenCleanupService
    {
        private readonly ILogger<TokenCleanupService> _logger;
        private readonly IOperationalStoreNotification _operationalStoreNotification;
        private readonly OperationalStoreOptions _options;
        private readonly IPersistedGrantDbContext _persistedGrantDbContext;

        public TokenCleanupService(
            OperationalStoreOptions options,
            IPersistedGrantDbContext persistedGrantDbContext,
            ILogger<TokenCleanupService> logger,
            IOperationalStoreNotification operationalStoreNotification = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            if (_options.TokenCleanupBatchSize < 1)
            {
                throw new ArgumentException("Token cleanup batch size interval must be at least 1");
            }

            _persistedGrantDbContext = persistedGrantDbContext ??
                                       throw new ArgumentNullException(nameof(persistedGrantDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _operationalStoreNotification = operationalStoreNotification;
        }

        public async Task RemoveExpiredGrantsAsync()
        {
            try
            {
                _logger.LogTrace("Querying for expired grants to remove");

                await RemoveGrantsAsync();
                await RemoveDeviceCodesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception removing expired grants: {exception}", ex.Message);
            }
        }

        protected virtual async Task RemoveGrantsAsync()
        {
            int found = int.MaxValue;

            while (found >= _options.TokenCleanupBatchSize)
            {
                DocumentSnapshot[] expiredGrants = (await _persistedGrantDbContext.PersistedGrants
                        .WhereLessThan(nameof(PersistedGrant.Expiration), DateTime.UtcNow)
                        .Limit(_options.TokenCleanupBatchSize)
                        .GetSnapshotAsync()
                        .ConfigureAwait(false))
                    .ToArray();

                found = expiredGrants.Length;
                _logger.LogInformation("Removing {grantCount} grants", found);

                if (found <= 0)
                {
                    continue;
                }

                IEnumerable<PersistedGrant> removedGrants = expiredGrants.Select(x => x.ConvertTo<PersistedGrant>());

                foreach (DocumentSnapshot expiredGrant in expiredGrants)
                {
                    await expiredGrant.Reference.DeleteAsync(Precondition.None);
                }

                if (_operationalStoreNotification != null)
                {
                    await _operationalStoreNotification.PersistedGrantsRemovedAsync(removedGrants);
                }
            }
        }

        protected virtual async Task RemoveDeviceCodesAsync()
        {
            int found = int.MaxValue;

            while (found >= _options.TokenCleanupBatchSize)
            {
                DocumentSnapshot[] expiredCodes = (await _persistedGrantDbContext.DeviceFlowCodes
                        .WhereLessThan(nameof(DeviceFlowCodes.Expiration), DateTime.UtcNow)
                        .Limit(_options.TokenCleanupBatchSize)
                        .GetSnapshotAsync()
                        .ConfigureAwait(false))
                    .ToArray();

                found = expiredCodes.Length;
                _logger.LogInformation("Removing {deviceCodeCount} device flow codes", found);

                if (found <= 0)
                {
                    continue;
                }

                IEnumerable<DeviceFlowCodes> removedCodes = expiredCodes.Select(x => x.ConvertTo<DeviceFlowCodes>());

                foreach (DocumentSnapshot expiredCode in expiredCodes)
                {
                    await expiredCode.Reference.DeleteAsync(Precondition.None);
                }

                if (_operationalStoreNotification != null)
                {
                    await _operationalStoreNotification.DeviceCodesRemovedAsync(removedCodes);
                }
            }
        }
    }
}