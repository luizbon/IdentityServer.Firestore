using System;
using IdentityServer4.Firestore.Storage.DbContexts;
using IdentityServer4.Firestore.Storage.Options;
using IdentityServer4.Firestore.Storage.TokenCleanup;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.Firestore.Storage.Configuration
{
    public static class IdentityServerFirestoreBuilderExtensions
    {
        public static IServiceCollection AddFirestore(this IServiceCollection services,
            Action<FirestoreOptions> firestoreOptionAction = null)
        {
            FirestoreOptions options = new FirestoreOptions();
            firestoreOptionAction?.Invoke(options);
            services.AddSingleton(options);

            return services;
        }

        public static IServiceCollection AddConfigurationDbContext(this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            return services.AddConfigurationDbContext<ConfigurationDbContext>(storeOptionsAction);
        }

        public static IServiceCollection AddConfigurationDbContext<TContext>(this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
            where TContext : FirestoreDbContext, IConfigurationDbContext
        {
            ConfigurationStoreOptions options = new ConfigurationStoreOptions();
            storeOptionsAction?.Invoke(options);
            services.AddSingleton(options);

            services.AddScoped<IConfigurationDbContext, TContext>();

            return services;
        }

        public static IServiceCollection AddOperationalDbContext(this IServiceCollection services,
            Action<OperationalStoreOptions> storeOptionsAction = null)
        {
            return services.AddOperationalDbContext<PersistedGrantDbContext>(storeOptionsAction);
        }

        public static IServiceCollection AddOperationalDbContext<TContext>(this IServiceCollection services,
            Action<OperationalStoreOptions> storeOptionsAction = null)
            where TContext : FirestoreDbContext, IPersistedGrantDbContext
        {
            OperationalStoreOptions storeOptions = new OperationalStoreOptions();
            storeOptionsAction?.Invoke(storeOptions);
            services.AddSingleton(storeOptions);

            services.AddScoped<IPersistedGrantDbContext, TContext>();
            services.AddTransient<TokenCleanupService>();

            return services;
        }

        public static IServiceCollection AddOperationalStoreNotification<T>(this IServiceCollection services)
            where T : class, IOperationalStoreNotification
        {
            services.AddTransient<IOperationalStoreNotification, T>();
            return services;
        }
    }
}