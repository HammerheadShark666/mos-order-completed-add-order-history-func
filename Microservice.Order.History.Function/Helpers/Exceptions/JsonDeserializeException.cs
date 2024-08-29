namespace Microservice.Order.History.Function.Helpers.Exceptions;

internal class JsonDeserializeException : Exception
{
    public JsonDeserializeException()
    {
    }

    public JsonDeserializeException(string message)
        : base(message)
    {
    }

    public JsonDeserializeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}