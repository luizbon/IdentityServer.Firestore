// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Firestore.Storage.DbContexts;
using IdentityServer4.Firestore.Storage.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ApiResource = IdentityServer4.Firestore.Storage.Entities.ApiResource;
using ApiScope = IdentityServer4.Firestore.Storage.Entities.ApiScope;
using Client = IdentityServer4.Firestore.Storage.Entities.Client;
using IdentityResource = IdentityServer4.Firestore.Storage.Entities.IdentityResource;

namespace Host.SeedData
{
    public static class SeedData
    {
        public static void EnsureSeedData(this IWebHost host)
        {
            using var scope = host.Services.CreateScope();

            var serviceProvider = scope.ServiceProvider;

            var context = serviceProvider.GetRequiredService<IConfigurationDbContext>();

            Task.Run(async () =>
            {
                await EnsureSeedData(context);
            }).Wait();
        }

        private static async Task EnsureSeedData(IConfigurationDbContext context)
        {
            Console.WriteLine("Seeding database...");

            if (!(await context.Clients.GetSnapshotAsync()).Any())
            {
                Console.WriteLine("Clients being populated");
                foreach (var client in Clients.Get())
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }
            }
            else
            {
                Console.WriteLine("Clients already populated");
            }

            if (!(await context.IdentityResources.GetSnapshotAsync()).Any())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in Resources.IdentityResources)
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!(await context.ApiResources.GetSnapshotAsync()).Any())
            {
                Console.WriteLine("ApiResources being populated");
                foreach (var resource in Resources.ApiResources)
                {
                    await context.ApiResources.AddAsync(resource.ToEntity());
                }
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }

            if (!(await context.ApiScopes.GetSnapshotAsync()).Any())
            {
                Console.WriteLine("Scopes being populated");
                foreach (var resource in Resources.ApiScopes)
                {
                    await context.ApiScopes.AddAsync(resource.ToEntity());
                }
            }
            else
            {
                Console.WriteLine("Scopes already populated");
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }
    }
}