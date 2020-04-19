using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Firestore.Storage.DbContexts;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Firestore.Storage.Stores
{
    public class ResourceStore: IResourceStore
    {
        private readonly IConfigurationDbContext _context;
        private readonly ILogger<ResourceStore> _logger;

        public ResourceStore(IConfigurationDbContext context, ILogger<ResourceStore> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var scopes = scopeNames.ToArray();

            var identityResources = (await _context
                    .IdentityResources
                    .WhereIn(nameof(IdentityResource.Name), scopes)
                    .GetSnapshotAsync()
                    .ConfigureAwait(false))
                .Where(x => x.Exists)
                .Select(x => x.ConvertTo<IdentityResource>())
                .ToArray();

            _logger.LogDebug("Found {scopes} identity scopes in database", identityResources.Select(x => x.Name));

            return identityResources;
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var scopes = scopeNames.ToArray();

            var apiScopes = (await _context.ApiScopes
                    .WhereIn(nameof(ApiScope.Name), scopes)
                    .GetSnapshotAsync()
                    .ConfigureAwait(false))
                .Where(x => x.Exists)
                .Select(x => x.ConvertTo<ApiScope>())
                .ToArray();

            _logger.LogDebug("Found {scopes} scopes in database", apiScopes.Select(x => x.Name));

            return apiScopes;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var apiResources = (await _context
                    .ApiResources
                    .WhereArrayContainsAny(nameof(ApiResource.Scopes), scopeNames)
                    .GetSnapshotAsync()
                    .ConfigureAwait(false))
                .Where(x => x.Exists)
                .Select(x => x.ConvertTo<ApiResource>())
                .ToList();

            _logger.LogDebug("Found {apis} API resources in database", apiResources.Select(x => x.Name));

            return apiResources;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            if (apiResourceNames == null) throw new ArgumentNullException(nameof(apiResourceNames));

            var apiResources = (await _context
                    .ApiResources
                    .WhereIn(nameof(ApiResource.Name), apiResourceNames)
                    .GetSnapshotAsync()
                    .ConfigureAwait(false))
                .Where(x => x.Exists)
                .Select(x => x.ConvertTo<ApiResource>())
                .ToList();

            if (apiResources.Any())
            {
                _logger.LogDebug("Found {apis} API resource in database", apiResources.Select(x => x.Name));
            }
            else
            {
                _logger.LogDebug("Did not find {apis} API resource in database", apiResourceNames);
            }

            return apiResources;
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var identity = (await _context.IdentityResources.GetSnapshotAsync()).Select(x => x.ConvertTo<IdentityResource>());
            var apis = (await _context.ApiResources.GetSnapshotAsync()).Select(x => x.ConvertTo<ApiResource>());
            var scopes = (await _context.ApiScopes.GetSnapshotAsync()).Select(x => x.ConvertTo<ApiScope>());

            var result = new Resources(identity, apis, scopes);
            
            _logger.LogDebug("Found {scopes} as all scopes, and {apis} as API resources",
                result.IdentityResources.Select(x => x.Name).Union(result.ApiScopes.Select(x => x.Name)),
                result.ApiResources.Select(x => x.Name));

            return result;
        }
    }
}
