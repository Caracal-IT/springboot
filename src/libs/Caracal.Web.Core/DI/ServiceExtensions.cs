namespace Caracal.Web.Core.DI;

public static class ServiceExtensions {
  public static IServiceCollection AddObjects(this IServiceCollection services, Type type) {
    type.Assembly
        .GetTypes()
        .Where(t => t.IsAssignableTo(typeof(IInjectable)))
        .ToList()
        .ForEach(Register);

    void Register(Type t) {
      var isolationLevel = services.GetIsolationLevel(t);
      
      t.GetInterfaces()
       .Where(a => !a.IsAssignableFrom(typeof(IInjectable)))
       .ToList()
       .ForEach(RegisterInterface);

      void RegisterInterface(Type i) => isolationLevel(i, t);
    }

    return services;
  }
  
  private static Func<Type, Type, IServiceCollection> GetIsolationLevel(this IServiceCollection services,  Type t) {
    if (t.IsAssignableTo(typeof(IScopedInjectable)))
      return services.AddScoped;
    if (t.IsAssignableTo(typeof(ITransientInjectable)))
      return services.AddTransient;
    if (t.IsAssignableTo(typeof(ISingletonInjectable)))
      return services.AddSingleton;

    return services.AddScoped;
  }
}