﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using IdentityServer4.Firestore.Storage.DbContexts;
using IdentityServer4.Firestore.Storage.Entities;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Firestore.Services
{
    public class CorsPolicyService: ICorsPolicyService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<CorsPolicyService> _logger;

        public CorsPolicyService(IHttpContextAccessor context, ILogger<CorsPolicyService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            origin = origin.ToLowerInvariant();

            // doing this here and not in the ctor because: https://github.com/aspnet/CORS/issues/105
            var dbContext = _context.HttpContext.RequestServices.GetRequiredService<IConfigurationDbContext>();
            
            var isAllowed = (await dbContext.Clients.WhereArrayContains(
                        new FieldPath(nameof(Client.AllowedCorsOrigins), nameof(ClientCorsOrigin.Origin)), origin)
                    .GetSnapshotAsync())
                .Any(x => x.Exists);

            _logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, isAllowed);

            return isAllowed;
        }
    }
}
