using Caracal.Web.MediatR.DI;
namespace Caracal.Web.MediatR.Extensions; 

public static class ServiceExtensions {
    public static IServiceCollection AddScopedServices(this IServiceCollection services, Type type) {
        type.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IService)))
            .ToList()
            .ForEach(RegisterServices);

        void RegisterServices(Type t) {
            t.GetInterfaces()
             .Where(a => a != typeof(IService))
             .ToList()
             .ForEach(RegisterInterface);
            
            void RegisterInterface(Type i) => services.AddScoped(i, t);
        }

        return services;
    }
}