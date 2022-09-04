// ReSharper disable ClassNeverInstantiated.Global
using Microsoft.AspNetCore.Http;

namespace Caracal.SpringBoot.Business.Handlers;

public record RegisterUserRequestParameters(int Id) : IHttpRequestParameters;
public record RegisterUserRequest(int Id, string FirstName) : IHttpPostRequest<RegisterUserRequestParameters>, IHttpRequest {
    public RegisterUserRequestParameters? Path { get; set; } = new(Id: 0);
}

[HttpPost("users/{id}")]
public class RegisterUserUseCase: IPostRequestHandler<RegisterUserRequestParameters, RegisterUserRequest> {
    public Task<IResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken) {
        return Task.FromResult(Ok($"Registered [{request.Path?.Id}] ({request.Id}) {request.FirstName}"));
    }
}