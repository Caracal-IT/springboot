namespace Caracal.Web.MediatR.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class HttpPostAttribute : HttpAttribute {
  public HttpPostAttribute(string path) { }
}