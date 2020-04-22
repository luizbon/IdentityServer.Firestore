using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Firestore.Storage.DbContexts;
using IdentityServer4.Firestore.Storage.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Firestore.Storage.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly IConfigurationDbContext _context;
        private readonly ILogger<ResourceStore> _logger;

        public ResourceStore(IConfigurationDbContext context, ILogger<ResourceStore> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(
            IEnumerable<string> scopeNames)
        {
            string[] scopes = scopeNames.ToArray();

            Entities.IdentityResource[] identityResources = (await _context
                    .IdentityResources
                    .WhereIn(nameof(IdentityResource.Name), scopes)
                    .GetSnapshotAsync()
                    .ConfigureAwait(false))
                .Where(x => x.Exists)
                .Select(x => x.ConvertTo<Entities.IdentityResource>())
                .ToArray();

            _logger.LogDebug("Found {scopes} identity scopes in database", identityResources.Select(x => x.Name));

            return identityResources.ToModel();
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            string[] scopes = scopeNames.ToArray();

            Entities.ApiScope[] apiScopes = (await _context.ApiScopes
                    .WhereIn(nameof(ApiScope.Name), scopes)
                    .GetSnapshotAsync()
                    .ConfigureAwait(false))
                .Where(x => x.Exists)
                .Select(x => x.ConvertTo<Entities.ApiScope>())
                .ToArray();

            _logger.LogDebug("Found {scopes} scopes in database", apiScopes.Select(x => x.Name));

            return apiScopes.ToModel();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null)
            {
                throw new ArgumentNullException(nameof(scopeNames));
            }

            List<Entities.ApiResource> apiResources = (await _context
                    .ApiResources
                    .WhereArrayContainsAny(nameof(ApiResource.Scopes), scopeNames)
                    .GetSnapshotAsync()
                    .ConfigureAwait(false))
                .Where(x => x.Exists)
                .Select(x => x.ConvertTo<Entities.ApiResource>())
                .ToList();

            _logger.LogDebug("Found {apis} API resources in database", apiResources.Select(x => x.Name));

            return apiResources.ToModel();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            if (apiResourceNames == null)
            {
                throw new ArgumentNullException(nameof(apiResourceNames));
            }

            List<Entities.ApiResource> apiResources = (await _context
                    .ApiResources
                    .WhereIn(nameof(ApiResource.Name), apiResourceNames)
                    .GetSnapshotAsync()
                    .ConfigureAwait(false))
                .Where(x => x.Exists)
                .Select(x => x.ConvertTo<Entities.ApiResource>())
                .ToList();

            if (apiResources.Any())
            {
                _logger.LogDebug("Found {apis} API resource in database", apiResources.Select(x => x.Name));
            }
            else
            {
                _logger.LogDebug("Did not find {apis} API resource in database", apiResourceNames);
            }

            return apiResources.ToModel();
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            IEnumerable<Entities.IdentityResource> identity =
                (await _context.IdentityResources.GetSnapshotAsync()).Select(x =>
                    x.ConvertTo<Entities.IdentityResource>());
            IEnumerable<Entities.ApiResource> apis =
                (await _context.ApiResources.GetSnapshotAsync()).Select(x => x.ConvertTo<Entities.ApiResource>());
            IEnumerable<Entities.ApiScope> scopes =
                (await _context.ApiScopes.GetSnapshotAsync()).Select(x => x.ConvertTo<Entities.ApiScope>());

            Resources result = new Resources(identity.ToModel(),
                apis.ToModel(),
                scopes.ToModel());

            _logger.LogDebug("Found {scopes} as all scopes, and {apis} as API resources",
                result.IdentityResources.Select(x => x.Name).Union(result.ApiScopes.Select(x => x.Name)),
                result.ApiResources.Select(x => x.Name));

            return result;
        }
    }
}