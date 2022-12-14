namespace Caracal.SpringBoot.Data;

public static class Plugin {
  public static IServiceCollection AddSpringBootData(this IServiceCollection services) {
    services.AddMediatR(x => x.AsScoped(), typeof(Plugin));
    services.AddObjects(typeof(Plugin));

    return services;
  }
}