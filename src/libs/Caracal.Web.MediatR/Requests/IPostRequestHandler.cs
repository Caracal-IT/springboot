namespace Caracal.Web.MediatR.Requests; 

public interface IPostRequestHandler<TPath, in TRequest> : IRequestHandler<TRequest, IResult> 
    where TPath: IHttpRequestParameters 
    where TRequest: IHttpRequest { }
