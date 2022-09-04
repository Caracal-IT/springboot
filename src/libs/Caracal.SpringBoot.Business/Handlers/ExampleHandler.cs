// ReSharper disable ClassNeverInstantiated.Global

using Microsoft.AspNetCore.Http;

namespace Caracal.SpringBoot.Business.Handlers;

public record ExampleRequest(string Age, string Name) : IHttpRequest;

public record ExampleResponse(string Message, Guid Id);

[HttpGet("example/{name}")]
public class ExampleHandler : IRequestHandler<ExampleRequest, IResult> {
  private readonly IGuidService _guidService;

  public ExampleHandler(IGuidService guidService) {
    _guidService = guidService;
  }

  public Task<IResult> Handle(ExampleRequest request, CancellationToken cancellationToken) {
    return Task.FromResult(Ok(new ExampleResponse($"The age was {request.Age} and the name was {request.Name}", _guidService.Id)));
  }
}