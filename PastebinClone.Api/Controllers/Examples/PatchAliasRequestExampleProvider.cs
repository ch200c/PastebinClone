using Swashbuckle.AspNetCore.Filters;

namespace PastebinClone.Api.Controllers.Examples;

public class PatchAliasRequestExampleProvider : IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new[]
        {
            new
            {
                op = "replace",
                path = "/alias",
                value = "abc"
            }
        };
    }
}
