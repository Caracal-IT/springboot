namespace Caracal.Web.Core.DI;

public interface IInjectable { }

public interface ITransientInjectable : IInjectable { }
public interface IScopedInjectable: IInjectable { }
public interface ISingletonInjectable: IInjectable { }
