using Caracal.Web.MediatR.DI;
namespace Caracal.Web.MediatR.Extensions; 

public static class ServiceExtensions {
    public static IServiceCollection AddScopedObjects(this IServiceCollection services, Type type) {
        type.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IScoped)))
            .ToList()
            .ForEach(Register);

        void Register(Type t) {
            t.GetInterfaces()
             .Where(a => a != typeof(IScoped))
             .ToList()
             .ForEach(RegisterInterface);
            
            void RegisterInterface(Type i) => services.AddScoped(i, t);
        }

        return services;
    }
}