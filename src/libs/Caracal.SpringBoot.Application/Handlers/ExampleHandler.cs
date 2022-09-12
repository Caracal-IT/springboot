// ReSharper disable ClassNeverInstantiated.Global
using Caracal.SpringBoot.Application.UseCases.Example;

namespace Caracal.SpringBoot.Application.Handlers;

public sealed record ExampleRequest(string Age, string Name) : IHttpRequest;

public sealed record ExampleResponse(string Message, Guid Id);

[HttpGet("example/{name}")]
public sealed class ExampleHandler : IRequestHandler<ExampleRequest, IResult> {
  private readonly IExampleUseCase _useCase;

  public ExampleHandler(IExampleUseCase useCase) =>
    _useCase = useCase;

    public async Task<IResult> Handle(ExampleRequest request, CancellationToken cancellationToken) {
    if (request.Name == "exception")
      throw new InvalidDataException("You cannot use exception in the example");
    
    var response = await _useCase.Execute(request.Adapt<PersonRequest>(), cancellationToken);
    
    return Ok(new ExampleResponse($"The age was {response.Age} and the name was {response.Name}", response.Id));
  }
}