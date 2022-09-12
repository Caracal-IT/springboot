namespace Caracal.Web.MediatR.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class HttpPostAttribute : HttpAttribute {
  public HttpPostAttribute(string path) { }
}