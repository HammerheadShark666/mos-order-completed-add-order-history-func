using FluentValidation.Results;
using Microservice.Order.History.Function.Helpers;

namespace Microservice.Order.History.Function.Test.Unit;

[TestFixture]
public class ErrorHelperTests
{

    [Test]
    public void Get_validation_fail_messages_return_list_of_messages()
    {
        var value1 = "value1";
        var value2 = "value2";

        IEnumerable<string> actualResult = ErrorHelper.GetErrorMessages(GetValidationFailures(value1, value2));

        Assert.Multiple(() =>
        {
            Assert.That(actualResult.Count, Is.EqualTo(2));
            Assert.That(actualResult.ElementAt(0), Is.EqualTo(value1));
            Assert.That(actualResult.ElementAt(1), Is.EqualTo(value2));
        });
    }

    [Test]
    public void Get_validation_fail_messages_return_string_of_messages()
    {
        var value1 = "value1";
        var value2 = "value2";

        string actualResult = ErrorHelper.GetErrorMessagesAsString(GetValidationFailures(value1, value2));

        Assert.That(actualResult, Is.EqualTo("[\"value1\",\"value2\"]"));
    }

    private static IEnumerable<ValidationFailure> GetValidationFailures(params string[] validationFailureValues)
    {
        var index = 1;

        foreach (var validationFailureValue in validationFailureValues)
        {
            yield return new ValidationFailure(string.Format("property{0}", index.ToString()), validationFailureValue);
            index++;
        }
    }
}