using Microsoft.AspNetCore.Http;

namespace Caracal.SpringBoot.Business.Handlers;

public record RegisterUserRequest(string FirstName) : IHttpRequest;

[HttpPost("users")]
public class RegisterUserUseCase: IRequestHandler<RegisterUserRequest, IResult> {
    public Task<IResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken) {
        return Task.FromResult(Ok($"Registered {request.FirstName}"));
    }
}