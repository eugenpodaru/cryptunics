namespace Microsoft.Extensions.DependencyInjection
{
    using Cryptunics.Core;
    using Cryptunics.Core.Repository;
    using Cryptunics.Infrastructure.Client.CoinMarketCap;
    using Cryptunics.Infrastructure.Client.ExchangeRates;
    using Cryptunics.Infrastructure.Repository;
    using Microsoft.Extensions.Options;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCryptunics(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureCryptunicsOptions(configuration);
            services.AddCryptunicsInfrastructure();
            services.AddCryptunicsCore();

            return services;
        }

        private static IServiceCollection AddCryptunicsCore(this IServiceCollection services)
        {
            services.AddScoped<ICoinManager, CoinManager>();
            services.AddScoped<IExchange, Exchange>();

            return services;
        }

        private static IServiceCollection AddCryptunicsInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICoinMarketCapClient, CoinMarketCapClient>();
            services.AddScoped<IExchangeRatesClient, ExchangeRatesClient>();
            services.AddScoped<ICoinRepository, CoinMarketCapCoinRepository>();
            services.AddScoped<ICryptoCoinQuoteRepository, CoinMarketCapCryptoCoinQuoteRepository>();
            services.AddScoped<IFiatCoinQuoteRepository, ExchangeRatesFiatCoinQuoteRepository>();

            return services;
        }

        private static IServiceCollection ConfigureCryptunicsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<CoinManagerOptions>(configuration);
            services.ConfigureOptions<CoinMarketCapOptions>(configuration);
            services.ConfigureOptions<ExchangeRatesOptions>(configuration);
            services.ConfigureOptions<CoinRepositoryOptions>(configuration);
            services.ConfigureOptions<CryptoCoinQuoteRepositoryOptions>(configuration);
            services.ConfigureOptions<FiatCoinQuoteRepositoryOptions>(configuration);

            return services;
        }

        private static IServiceCollection ConfigureOptions<T>(this IServiceCollection services, IConfiguration configuration) where T : class
        {
            services.Configure<T>(configuration.GetSection<T>());
            services.AddScoped(x => x.GetService<IOptionsSnapshot<T>>()!.Value);

            return services;
        }

        private static IConfigurationSection GetSection<T>(this IConfiguration configuration)
        {
            const string sectionNameSuffix = "Options";
            var optionsName = typeof(T).Name;
            var key = optionsName.EndsWith(sectionNameSuffix, StringComparison.OrdinalIgnoreCase)
                ? optionsName.Remove(optionsName.LastIndexOf(sectionNameSuffix, StringComparison.OrdinalIgnoreCase))
                : optionsName;

            return configuration.GetSection(key);
        }
    }
}
