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
        .FirstOrDefault(a => a.AttributeType.IsAssignableTo(typeof(HttpAttribute)));

      if (verbAttr == null)
        return;

      var requestType = handlerType.GetInterfaces()
        .First(i => i.Name.StartsWith("IRequestHandler"))
        .GetGenericArguments()
        .First();

      var pathType = handlerType.GetInterfaces()
        .First().GenericTypeArguments
        .First();

      var requestTypes = pathType == requestType
        ? new[] {requestType}
        : new[] {pathType, requestType};

      typeof(HandlerExtensions).GetMethod(verbAttr.AttributeType.Name)!
        .MakeGenericMethod(requestTypes.ToArray())
        .Invoke(null, new[] {app, verbAttr.ConstructorArguments.First().Value});
    }
  }

  public static void HttpGetAttribute<TRequest>(WebApplication app, string template) where TRequest : IHttpRequest {
    app.MapGet(template, async (IMediator mediator, [AsParameters] TRequest req) => await mediator.Send(req));
  }

  public static void HttpPostAttribute<TPath, TRequest>(WebApplication app, string template)
    where TPath : IHttpRequestParameters
    where TRequest : IHttpPostRequest<TPath> {
    app.MapPost(template, async (IMediator mediator, [AsParameters] TPath path, TRequest req) => {
      req.Path = path;
      return await mediator.Send(req);
    });
  }
}