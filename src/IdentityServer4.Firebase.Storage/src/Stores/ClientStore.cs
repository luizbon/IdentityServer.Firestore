using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Firestore.Storage.DbContexts;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Mapster;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Firestore.Storage.Stores
{
    public class ClientStore: IClientStore
    {
        private readonly IConfigurationDbContext _context;
        private readonly ILogger<ClientStore> _logger;

        public ClientStore(IConfigurationDbContext context, ILogger<ClientStore> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = (await _context.Clients
                .WhereEqualTo(nameof(Client.ClientId), clientId)
                .Limit(1)
                .GetSnapshotAsync().ConfigureAwait(false)).First();

            if (!client.Exists) return null;

            var model = client.ConvertTo<Entities.Client>().Adapt<Client>();

            _logger.LogDebug("{clientId} found in database: {clientIdFound}", clientId, model != null);

            return model;
        }
    }
}
