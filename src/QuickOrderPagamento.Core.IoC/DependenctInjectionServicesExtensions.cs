using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickOrderPagamento.Adapters.Driven.RabbitMQ.Configuration;
using System.Reflection;

namespace QuickOrderPagamento.Core.IoC
{
    public static class DependenctInjectionServicesExtensions
    {
        public static void AddImplementations(this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            Type baseInterface,
            IEnumerable<Type>? assemblyTypesParam = null)
        {
            var assemblyTypes = assemblyTypesParam ?? baseInterface.Assembly.GetNoAbstractTypes();

            foreach (var assemblyType in assemblyTypes)
            {
                var baseInterfaceType = baseInterface;
                var typeInterfaces = assemblyType.GetInterfaces();
                var implementsInterface = typeInterfaces.Any(p => p.AssemblyQualifiedName == baseInterfaceType.AssemblyQualifiedName);

                if (implementsInterface)
                {
                    var typeInterface = typeInterfaces.FirstOrDefault(t => t.GetInterfaces().Contains(baseInterfaceType));

                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(typeInterface, assemblyType);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(typeInterface, assemblyType);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(typeInterface, assemblyType);
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQSettings = new RabbitMQSettings();
            configuration.GetSection("RabbitMQ").Bind(rabbitMQSettings);

            services.AddSingleton(rabbitMQSettings);

            var assemblyTypes = Assembly.GetExecutingAssembly().GetNoAbstractTypes();
            var rabbitMQTypes = assemblyTypes.Where(t => t.Namespace == "QuickOrderPagamento.Adapters.Driven.RabbitMQ.Publishers" ||
                                                         t.Namespace == "QuickOrderPagamento.Adapters.Driven.RabbitMQ.Subscribers");

            foreach (var type in rabbitMQTypes)
            {
                var interfaces = type.GetInterfaces();
                foreach (var iface in interfaces)
                {
                    services.AddTransient(iface, type);
                }
            }
        }
    }

    public static class AssemblyExtentions
    {
        public static IEnumerable<Type> GetNoAbstractTypes(this Assembly assemblyFile)
        {
            var types = assemblyFile.GetTypes();
            var assemblyTypes = types.Where(t => !t.GetTypeInfo().IsAbstract);
            return assemblyTypes;
        }
    }
}
