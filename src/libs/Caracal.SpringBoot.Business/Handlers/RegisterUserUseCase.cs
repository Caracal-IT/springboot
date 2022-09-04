// ReSharper disable ClassNeverInstantiated.Global
using Microsoft.AspNetCore.Http;

namespace Caracal.SpringBoot.Business.Handlers;

public record RegisterUserRequestParameters(int Id) : IHttpRequest;
public record RegisterUserRequest(int Id, string FirstName) : IHttpRequest;

[HttpPost("users/{id}")]
public class RegisterUserUseCase: IRequestHandler<RegisterUserRequest, IResult> {
    public Task<IResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken) {
        return Task.FromResult(Ok($"Registered ({request.Id}) {request.FirstName}"));
    }
}