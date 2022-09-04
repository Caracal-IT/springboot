using Microsoft.Extensions.DependencyInjection;

namespace Caracal.Web.Core.DI;

public static class ServiceExtensions {
  public static IServiceCollection AddObjects(this IServiceCollection services, Func<Type, Type, IServiceCollection> isolationLevel, Type type) {
    type.Assembly
        .GetTypes()
        .Where(t => t.IsAssignableTo(typeof(IInjectable)))
        .ToList()
        .ForEach(Register);

    void Register(Type t) {
      t.GetInterfaces()
       .Where(a => a != typeof(IInjectable))
       .ToList()
       .ForEach(RegisterInterface);

      void RegisterInterface(Type i) => isolationLevel(i, t);
    }

    return services;
  }
}