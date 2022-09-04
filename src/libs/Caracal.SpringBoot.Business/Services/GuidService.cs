namespace Caracal.SpringBoot.Business.Services; 

public class GuidService: IGuidService, IScoped {
    public Guid Id { get; set; } = Guid.NewGuid();
}