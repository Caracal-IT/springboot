namespace Caracal.Web.MediatR.Attributes; 

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]  
public class HttpPostAttribute : Attribute {
    public HttpPostAttribute(string path) { }
}