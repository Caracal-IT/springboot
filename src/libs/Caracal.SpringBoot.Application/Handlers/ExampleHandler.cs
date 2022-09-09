// ReSharper disable ClassNeverInstantiated.Global

using Caracal.SpringBoot.Application.Repositories;
using Caracal.SpringBoot.Application.UseCases.Example;

namespace Caracal.SpringBoot.Application.Handlers;

public record ExampleRequest(string Age, string Name) : IHttpRequest;

public record ExampleResponse(string Message, Guid Id);

[HttpGet("example/{name}")]
public class ExampleHandler : IRequestHandler<ExampleRequest, IResult> {
  private readonly IExampleUseCase _useCase;
  private readonly IDataContext _dataContext;

  public ExampleHandler(IExampleUseCase useCase, IDataContext dataContext) {
    _useCase = useCase;
    _dataContext = dataContext;
  }

  public async Task<IResult> Handle(ExampleRequest request, CancellationToken cancellationToken) {
    if (request.Name == "exception")
      throw new InvalidDataException("You cannot use exception in the example");
    
    var response = await _useCase.Execute(request.Adapt<PersonRequest>(), cancellationToken);
    
    return Ok(new ExampleResponse($" {_dataContext.GetData().FirstOrDefault()} The age was {response.Age} and the name was {response.Name}", response.Id));
  }
}