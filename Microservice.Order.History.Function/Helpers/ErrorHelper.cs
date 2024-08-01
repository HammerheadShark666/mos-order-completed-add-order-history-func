using FluentValidation.Results;
using System.Text.Json;

namespace Microservice.Order.History.Function.Helpers;

public class ErrorHelper
{
    public static IEnumerable<String> GetErrorMessages(IEnumerable<ValidationFailure> failures)
    {
        foreach (var failure in failures)
        {
            yield return failure.ErrorMessage;
        }
    }

    public static string GetErrorMessagesAsString(IEnumerable<ValidationFailure> failures)
    {
        return JsonSerializer.Serialize(GetErrorMessages(failures));
    }
}