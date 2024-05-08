using Microsoft.Extensions.DependencyInjection;
using QuickOrderPagamento.Adapters.Driven.MercadoPago;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Interfaces;
using QuickOrderPagamento.Adapters.Driven.MongoDB.Core;
using QuickOrderPagamento.Adapters.Driven.MongoDB.Repositories;
using QuickOrderPagamento.Core.Application.UseCases;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces;
using QuickOrderPagamento.Core.Domain.Adapters;
using QuickOrderPagamento.Core.Domain.Repositories;

namespace QuickOrderPagamento.Core.IoC
{
    public static class RootBootstrapper
    {
        public static void BootstrapperRegisterServices(this IServiceCollection services)
        {
            var assemblyTypes = typeof(RootBootstrapper).Assembly.GetNoAbstractTypes();

            services.AddImplementations(ServiceLifetime.Scoped, typeof(IBaseRepository), assemblyTypes);

            services.AddImplementations(ServiceLifetime.Scoped, typeof(IBaseUseCase), assemblyTypes);

            //Repositories MongoDB
            services.AddSingleton<IMondoDBContext, MondoDBContext>();
            services.AddScoped<IPagamentoStatusRepository, PagamentoStatusRepository>();


            //UseCases
            services.AddScoped<IPagamentoUseCase, PagamentoUseCase>();

            services.AddScoped<IMercadoPagoApi, MercadoPagoApi>();
        }
    }
}
