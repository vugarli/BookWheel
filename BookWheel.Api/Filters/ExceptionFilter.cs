using BookWheel.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Models;
using System.Collections;
using System.Dynamic;
using System.Text.Json;
using BookWheel.Domain.Exceptions;

namespace BookWheel.Api.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;
        public ExceptionFilter()
        {
            jsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions();

            if (jsonSerializerOptions.DictionaryKeyPolicy == null)
            {
                jsonSerializerOptions.DictionaryKeyPolicy = jsonSerializerOptions.PropertyNamingPolicy;
            }
        }

        private static string ApplySerialization(DictionaryEntry error, JsonSerializerOptions jsonSerializerOptions)
        {
            IDictionary<string, Object> interimObject = new ExpandoObject();
            interimObject.Add(error.Key.ToString(), error.Value);

            string serialisedInterimObject =
                JsonSerializer.Serialize(interimObject, jsonSerializerOptions);

            var deserializedError =
                JsonSerializer.Deserialize<IDictionary<string, Object>>(
                    serialisedInterimObject,
                    jsonSerializerOptions);

            return deserializedError.Keys.First();
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is ModelValidationException)
            {
                var problemDetail = new ValidationProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = context.Exception.Message
                };

                foreach (DictionaryEntry error in context.Exception.Data)
                {
                    string errorKey = ApplySerialization(error, jsonSerializerOptions);

                    problemDetail.Errors.Add(
                        key: errorKey,
                        value: ((List<string>)error.Value)?.ToArray());
                }

                context.Result = new BadRequestObjectResult(problemDetail);
            }
            else if (context.Exception is DomainException)
            {
                var problemDetail = new ValidationProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = context.Exception.Message
                };

                foreach (DictionaryEntry error in context.Exception.Data)
                {
                    string errorKey = ApplySerialization(error, jsonSerializerOptions);

                    problemDetail.Errors.Add(
                        key: errorKey,
                        value: ((List<string>)error.Value)?.ToArray());
                }

                context.Result = new BadRequestObjectResult(problemDetail);
            }
            //else
            //{
            //    var problemDetail = new ValidationProblemDetails
            //    {
            //        Status = StatusCodes.Status500InternalServerError,
            //        Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            //        Title = "Something bad happened"
            //    };

            //    context.Result = new InternalServerErrorObjectResult(problemDetail);
            //}
        }
    }
}
