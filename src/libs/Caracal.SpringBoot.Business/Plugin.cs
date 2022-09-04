using Microsoft.Extensions.DependencyInjection;
using Caracal.Web.MediatR.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Caracal.SpringBoot.Business; 

public static class Plugin {
    public static IServiceCollection AddSpringBoot(this IServiceCollection services) {
        services.AddMediatR(x => x.AsScoped(), typeof(Plugin));
        services.AddScopedServices(typeof(Plugin));

        return services;
    }

    public static WebApplication UseSpringBoot(this WebApplication app) {
        app.RegisterHandlers(typeof(Plugin));
        return app;
    }
}