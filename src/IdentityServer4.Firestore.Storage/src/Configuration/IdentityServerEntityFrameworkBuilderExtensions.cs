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
            var options = new FirestoreOptions();
            services.AddSingleton(options);
            firestoreOptionAction?.Invoke(options);

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
            var options = new ConfigurationStoreOptions();
            services.AddSingleton(options);
            storeOptionsAction?.Invoke(options);

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
            var storeOptions = new OperationalStoreOptions();
            services.AddSingleton(storeOptions);
            storeOptionsAction?.Invoke(storeOptions);
            
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
