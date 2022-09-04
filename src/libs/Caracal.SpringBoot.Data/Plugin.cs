using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Caracal.SpringBoot.Data;

public static class Plugin {
  public static IServiceCollection AddSpringBootData(this IServiceCollection services) {
    services.AddMediatR(x => x.AsScoped(), typeof(Plugin));
    services.AddObjects(services.AddScoped, typeof(Plugin));

    return services;
  }
}