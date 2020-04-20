using System;
using IdentityServer4.Firestore.Services;
using IdentityServer4.Firestore.Storage.Configuration;
using IdentityServer4.Firestore.Storage.DbContexts;
using IdentityServer4.Firestore.Storage.Options;
using IdentityServer4.Firestore.Storage.Stores;
using IdentityServer4.Firestore.Storage.TokenCleanup;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer4.Firestore
{
    public static class IdentityServerFirestoreBuilderExtensions
    {
        public static IIdentityServerBuilder AddFirestore(this IIdentityServerBuilder builder,
            Action<FirestoreOptions> storeOptionsAction = null)
        {
            builder.Services.AddFirestore(storeOptionsAction);

            return builder;
        }

        public static IIdentityServerBuilder AddConfigurationStore(this IIdentityServerBuilder builder,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            return builder.AddConfigurationStore<ConfigurationDbContext>(storeOptionsAction);
        }

        public static IIdentityServerBuilder AddConfigurationStore<TContext>(
            this IIdentityServerBuilder builder,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
            where TContext : FirestoreDbContext, IConfigurationDbContext
        {
            builder.Services.AddConfigurationDbContext<TContext>(storeOptionsAction);

            builder.AddClientStore<ClientStore>();
            builder.AddResourceStore<ResourceStore>();
            builder.AddCorsPolicyService<CorsPolicyService>();

            return builder;
        }

        public static IIdentityServerBuilder AddConfigurationStoreCache(
            this IIdentityServerBuilder builder)
        {
            builder.AddInMemoryCaching();

            // add the caching decorators
            builder.AddClientStoreCache<ClientStore>();
            builder.AddResourceStoreCache<ResourceStore>();
            builder.AddCorsPolicyCache<CorsPolicyService>();

            return builder;
        }

        public static IIdentityServerBuilder AddOperationalStore(
            this IIdentityServerBuilder builder,
            Action<OperationalStoreOptions> storeOptionsAction = null)
        {
            return builder.AddOperationalStore<PersistedGrantDbContext>(storeOptionsAction);
        }

        public static IIdentityServerBuilder AddOperationalStore<TContext>(
            this IIdentityServerBuilder builder,
            Action<OperationalStoreOptions> storeOptionsAction = null)
            where TContext : FirestoreDbContext, IPersistedGrantDbContext
        {
            builder.Services.AddOperationalDbContext<TContext>(storeOptionsAction);

            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            builder.Services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();
            builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();

            return builder;
        }

        public static IIdentityServerBuilder AddOperationalStoreNotification<T>(
            this IIdentityServerBuilder builder)
            where T : class, IOperationalStoreNotification
        {
            builder.Services.AddOperationalStoreNotification<T>();
            return builder;
        }
    }
}
