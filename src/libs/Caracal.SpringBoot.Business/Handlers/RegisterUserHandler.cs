// ReSharper disable ClassNeverInstantiated.Global
namespace Caracal.SpringBoot.Business.Handlers;

public record RegisterUserRequestParameters(int Id) : IHttpRequestParameters;

public record RegisterUserRequest(int Id, string FirstName) : IHttpPostRequest<RegisterUserRequestParameters>, IHttpRequest {
  public RegisterUserRequestParameters? Path { get; set; } = new(0);
}

[HttpPost("users/{id}")]
public class RegisterUserHandler : IPostRequestHandler<RegisterUserRequestParameters, RegisterUserRequest> {
  public Task<IResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken) =>
    Task.FromResult(Ok($"Registered [{request.Path?.Id}] ({request.Id}) {request.FirstName}"));
}