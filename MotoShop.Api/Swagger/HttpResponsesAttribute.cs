namespace MotoShop.Api.Swagger
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HttpResponsesAttribute : Attribute
    {
        public string Description { get; }

        public HttpResponsesAttribute(string description)
        {
            Description = description;
        }
    }
}
