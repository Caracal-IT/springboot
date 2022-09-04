using Microsoft.AspNetCore.Mvc;
using HttpGetAttribute = Caracal.Web.MediatR.Attributes.HttpGetAttribute;
using HttpPostAttribute = Caracal.Web.MediatR.Attributes.HttpPostAttribute;

namespace Caracal.Web.MediatR.Extensions; 

public static class HandlerExtensions {
    public static void RegisterHandlers(this WebApplication app, Type handlerAssembly) {
        handlerAssembly.Assembly
                       .GetTypes()
                       .Where(t => t.GetInterfaces().Any(i => i.Name.StartsWith("IRequestHandler")))
                       .ToList()
                       .ForEach(RegisterHandler);
            
        void RegisterHandler(Type handlerType) {
            var verbAttr = handlerType.CustomAttributes
                                      .FirstOrDefault(
                                          a => a.AttributeType == typeof(HttpGetAttribute) ||
                                               a.AttributeType == typeof(HttpPostAttribute));

            if (verbAttr == null)
                return;
         
            var handlerInterface = handlerType.GetInterfaces()
                                              .First(i => i.Name.StartsWith("IRequestHandler"));

            var requestType = handlerInterface.GetGenericArguments()
                                              .First();
            
            typeof(HandlerExtensions).GetMethod(verbAttr.AttributeType.Name!)!
                                     .MakeGenericMethod(requestType)
                                     .Invoke(null, new[] {app, verbAttr.ConstructorArguments.First().Value});
        }
    }
    
    public static void HttpGetAttribute<TRequest>(WebApplication app, string template) where TRequest: IHttpRequest =>
        app.MapGet(template, async (IMediator mediator, [AsParameters] TRequest req) => await mediator.Send(req));
    
    public static void HttpPostAttribute<TRequest>(WebApplication app, string template) where TRequest: IHttpRequest =>
        app.MapPost(template, async (IMediator mediator, [FromBody] TRequest req) => await mediator.Send(req));
}