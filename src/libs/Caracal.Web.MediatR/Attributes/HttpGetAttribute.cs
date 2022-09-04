namespace Caracal.Web.MediatR.Attributes; 

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]  
public class HttpGetAttribute : Attribute {
    public HttpGetAttribute(string path) { }
}