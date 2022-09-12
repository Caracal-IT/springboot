// ReSharper disable ClassNeverInstantiated.Global
namespace Caracal.SpringBoot.Application.Handlers;

public sealed record RegisterUserRequestParameters(int Id) : IHttpRequestParameters;

public sealed record RegisterUserRequest(int Id, string FirstName) : IHttpPostRequest<RegisterUserRequestParameters>, IHttpRequest {
  public RegisterUserRequestParameters? Path { get; set; } = new(0);
}

[HttpPost("users/{id}")]
public sealed class RegisterUserHandler : IPostRequestHandler<RegisterUserRequestParameters, RegisterUserRequest> {
  public Task<IResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken) =>
    Task.FromResult(Ok($"Registered [{request.Path?.Id}] ({request.Id}) {request.FirstName}"));
}