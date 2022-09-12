namespace Caracal.Web.MediatR.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class HttpGetAttribute : HttpAttribute {
  public HttpGetAttribute(string path) { }
}