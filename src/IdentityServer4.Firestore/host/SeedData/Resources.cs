// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Host.SeedData
{
    public class Resources
    {
        public static IEnumerable<IdentityResource> IdentityResources =
            new[]
            {
                // some standard scopes from the OIDC spec
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),

                // custom identity resource with some consolidated claims
                new IdentityResource("custom.profile", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, "location" })
            };

        public static IEnumerable<ApiResource> ApiResources = new[]
        {
            // simple version with ctor
            new ApiResource("api1", "Some API 1")
            {
                //// this is needed for introspection when using reference tokens
                //ApiSecrets = { new Secret("secret".Sha256()) },

                //AllowedSigningAlgorithms = { "RS256", "ES256" }

                Scopes = { "feature1" }
            },
                
            // expanded version if more control is needed
            new ApiResource
            {
                Name = "api2",

                ApiSecrets =
                {
                    new Secret("secret".Sha256())
                },

                //AllowedSigningAlgorithms = { "PS256", "ES256", "RS256" },

                UserClaims =
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Email
                },

                Scopes = { "feature2", "feature3" }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes = new[]
        {
            // local API
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
            new ApiScope("feature1"),
            new ApiScope("feature2"),
            new ApiScope("feature3"),
            new ApiScope
            {
                Name = "transaction"
            }
        };
    }
}