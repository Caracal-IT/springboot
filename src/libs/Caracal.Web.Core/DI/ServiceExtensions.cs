using Microsoft.Extensions.DependencyInjection;

namespace Caracal.Web.Core.DI;

public static class ServiceExtensions {
  public static IServiceCollection AddObjects(this IServiceCollection services, Func<Type, Type, IServiceCollection> scope, Type type) {
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

      void RegisterInterface(Type i) => scope(i, t);
    }

    return services;
  }
}