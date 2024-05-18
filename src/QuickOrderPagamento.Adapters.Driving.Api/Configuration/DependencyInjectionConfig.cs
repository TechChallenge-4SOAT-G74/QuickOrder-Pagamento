
using QuickOrderPagamento.Core.IoC;

namespace QuickOrderPagamento.Adapters.Driving.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            RootBootstrapper.BootstrapperRegisterServices(services);
        }
    }
}
