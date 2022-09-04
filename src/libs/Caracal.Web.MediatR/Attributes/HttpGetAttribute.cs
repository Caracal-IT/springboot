namespace Caracal.Web.MediatR.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class HttpGetAttribute : HttpAttribute {
  public HttpGetAttribute(string path) { }
}