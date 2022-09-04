namespace Caracal.SpringBoot.Business.Services; 

public class GuidService: IGuidService, IService {
    public Guid Id { get; set; } = Guid.NewGuid();
}