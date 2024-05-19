using Microsoft.Extensions.DependencyInjection;
using QuickOrderPagamento.Adapters.Driven.MercadoPago;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Interfaces;
using QuickOrderPagamento.Adapters.Driven.MongoDB.Core;
using QuickOrderPagamento.Adapters.Driven.MongoDB.Repositories;
using QuickOrderPagamento.Core.Application.UseCases;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces;
using QuickOrderPagamento.Core.Application.UseCases.Pedido.Interfaces;
using QuickOrderPagamento.Core.Application.UseCases.Pedido;
using QuickOrderPagamento.Core.Domain.Adapters;
using QuickOrderPagamento.Core.Domain.Repositories;
using QuickOrderPagamento.Infrastructure.Data;
using QuickOrderPagamento.Infra.MQ;

namespace QuickOrderPagamento.Core.IoC
{
    public static class RootBootstrapper
    {
        public static void BootstrapperRegisterServices(this IServiceCollection services)
        {
            var assemblyTypes = typeof(RootBootstrapper).Assembly.GetNoAbstractTypes();

            services.AddSingleton(typeof(IRabbitMqPub<>), typeof(RabbitMqPub<>));
            services.AddSingleton<IProcessaEvento, ProcessaEvento>();

            services.AddImplementations(ServiceLifetime.Scoped, typeof(IBaseRepository), assemblyTypes);

            services.AddImplementations(ServiceLifetime.Scoped, typeof(IBaseUseCase), assemblyTypes);

            //Repositories MongoDB
            services.AddSingleton<IMongoDBContext, MongoDBContext>();
            services.AddScoped<IPagamentoStatusRepository, PagamentoStatusRepository>();

            //UseCases
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPagamentoAtualizarUseCase, PagamentoAtualizarUseCase>();
            services.AddScoped<IPagamentoCriarUseCase, PagamentoCriarUseCase>();
            services.AddScoped<IPagamentoObterUseCase, PagamentoObterUseCase>();
            services.AddScoped<IPagamentoUseCase, PagamentoUseCase>();
            services.AddScoped<IMercadoPagoApi, MercadoPagoApi>();
        }
    }
}
