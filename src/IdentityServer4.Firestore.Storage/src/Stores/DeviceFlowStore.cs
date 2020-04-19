using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using IdentityModel;
using IdentityServer4.Firestore.Storage.DbContexts;
using IdentityServer4.Firestore.Storage.Entities;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Firestore.Storage.Stores
{
    public class DeviceFlowStore: IDeviceFlowStore
    {
        private readonly IPersistedGrantDbContext _context;
        private readonly IPersistentGrantSerializer _serializer;
        private readonly ILogger<DeviceFlowStore> _logger;

        public DeviceFlowStore(IPersistedGrantDbContext context,
            IPersistentGrantSerializer serializer,
            ILogger<DeviceFlowStore> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data)
        {
            await _context.DeviceFlowCodes.AddAsync(ToEntity(data, deviceCode, userCode)).ConfigureAwait(false);
        }

        public async Task<DeviceCode> FindByUserCodeAsync(string userCode)
        {
            var deviceFlowCodes = await GetDeviceFlow(nameof(DeviceFlowCodes.UserCode), userCode).ConfigureAwait(false);

            if (!deviceFlowCodes.Exists) return null;

            var model = ToModel(deviceFlowCodes.ConvertTo<DeviceFlowCodes>().Data);

            _logger.LogDebug("{userCode} found in database: {userCodeFound}", userCode, model != null);

            return model;
        }

        public async Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
            var deviceFlowCodes = await GetDeviceFlow(nameof(DeviceFlowCodes.DeviceCode), deviceCode).ConfigureAwait(false);

            if (!deviceFlowCodes.Exists) return null;

            var model = ToModel(deviceFlowCodes.ConvertTo<DeviceFlowCodes>().Data);

            _logger.LogDebug("{deviceCode} found in database: {deviceCodeFound}", deviceCode, model != null);

            return model;
        }

        public async Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
        {
            var deviceFlowCodes = await GetDeviceFlow(nameof(DeviceFlowCodes.UserCode), userCode).ConfigureAwait(false);
            if (!deviceFlowCodes.Exists)
            {
                _logger.LogError("{userCode} not found in database", userCode);
                throw new InvalidOperationException("Could not update device code");
            }

            var existing = deviceFlowCodes.ConvertTo<DeviceFlowCodes>();

            var entity = ToEntity(data, existing.DeviceCode, userCode);
            _logger.LogDebug("{userCode} found in database", userCode);

            existing.SubjectId = data.Subject?.FindFirst(JwtClaimTypes.Subject).Value;
            existing.Data = entity.Data;

            await deviceFlowCodes.Reference.SetAsync(existing).ConfigureAwait(false);
        }

        public async Task RemoveByDeviceCodeAsync(string deviceCode)
        {
            var deviceFlowCodes = await GetDeviceFlow(nameof(DeviceFlowCodes.DeviceCode), deviceCode).ConfigureAwait(false);

            if (deviceFlowCodes.Exists)
            {
                _logger.LogDebug("removing {deviceCode} device code from database", deviceCode);
                await deviceFlowCodes.Reference.DeleteAsync(Precondition.None).ConfigureAwait(false);
            }
            else
            {
                _logger.LogDebug("no {deviceCode} device code found in database", deviceCode);
            }
        }

        private async Task<DocumentSnapshot> GetDeviceFlow(string property, string value)
        {
            return (await _context.DeviceFlowCodes.WhereEqualTo(property, value).Limit(1)
                .GetSnapshotAsync().ConfigureAwait(false)).First();
        }

        protected DeviceFlowCodes ToEntity(DeviceCode model, string deviceCode, string userCode)
        {
            if (model == null || deviceCode == null || userCode == null) return null;

            return new DeviceFlowCodes
            {
                DeviceCode = deviceCode,
                UserCode = userCode,
                ClientId = model.ClientId,
                SubjectId = model.Subject?.FindFirst(JwtClaimTypes.Subject).Value,
                CreationTime = model.CreationTime,
                Expiration = model.CreationTime.AddSeconds(model.Lifetime),
                Data = _serializer.Serialize(model)
            };
        }
        protected DeviceCode ToModel(string entity)
        {
            return entity == null ? 
                null : 
                _serializer.Deserialize<DeviceCode>(entity);
        }
    }
}
