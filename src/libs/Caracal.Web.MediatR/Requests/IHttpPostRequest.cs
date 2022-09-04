namespace Caracal.Web.MediatR.Requests;

public interface IHttpPostRequest<TPath> : IRequest<IResult> {
    TPath? Path { get; set; }
}