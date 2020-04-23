// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Host.Extensions;
using Host.QuickStart.UI;
using IdentityServer4.Firestore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(TestUsers.Users)
                .AddFirestore(options =>
                {
                    options.ProjectId = "identity-server";
                })
                // this adds the config data from DB (clients, resources, CORS)
                .AddConfigurationStore()
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 5; // interval in seconds, short for testing
                });
            // this is something you will want in production to reduce load on and requests to the DB
            //.AddConfigurationStoreCache();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<RequestLoggerMiddleware>();

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
