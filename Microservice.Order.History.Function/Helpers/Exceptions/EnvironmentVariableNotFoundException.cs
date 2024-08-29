namespace Microservice.Order.History.Function.Helpers.Exceptions;

public class EnvironmentVariableNotFoundException : Exception
{
    public EnvironmentVariableNotFoundException()
    {
    }

    public EnvironmentVariableNotFoundException(string message)
        : base(message)
    {
    }

    public EnvironmentVariableNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}