namespace Caracal.SpringBoot.Business;

public static class Plugin {
  public static IServiceCollection AddSpringBoot(this IServiceCollection services) {
    services.AddMediatR(x => x.AsScoped(), typeof(Plugin));
    services.AddObjects(typeof(Plugin));

    return services;
  }

  public static WebApplication UseSpringBoot(this WebApplication app) {
    app.RegisterHandlers(typeof(Plugin));

    return app;
  }
}