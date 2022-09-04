namespace Caracal.SpringBoot.Business.UseCases.Example;

public interface IExampleUseCase {
  public Task<PersonResponse> Execute(PersonRequest request, CancellationToken cancellationToken);
}

public class ExampleUseCase: IExampleUseCase, ITransientInjectable {
  private readonly IGuidService _guidService;
  
  public ExampleUseCase(IGuidService guidService) =>
    _guidService = guidService;

  public Task<PersonResponse> Execute(PersonRequest request, CancellationToken cancellationToken) =>
    Task.FromResult(new PersonResponse(_guidService.Id, request.Age, request.Name));
}

public record PersonRequest(Guid Id, string Age, string Name) {
  public PersonRequest() : this(Guid.Empty, string.Empty, string.Empty) {}
}
public record PersonResponse(Guid Id, string Age, string Name);