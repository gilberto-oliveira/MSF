using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Builder
{
    public static class ProblemDetailsExtensions
    {
        public static void UseProblemDetailsExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandlerFeature != null)
                    {
                        var execeptionCatched = exceptionHandlerFeature.Error;

                        var problemDetails = new ProblemDetails
                        {
                            Instance = context.Request.HttpContext.Request.Path
                        };

                        if (execeptionCatched is BadHttpRequestException badHttpRequestException)
                        {
                            problemDetails.Title = "Requisição inválida";
                            problemDetails.Status = StatusCodes.Status400BadRequest;
                            problemDetails.Detail = badHttpRequestException.Message;
                        }
                        else if (execeptionCatched is SecurityTokenExpiredException securityTokenExpiredException)
                        {
                            problemDetails.Title = "Sessão expirada";
                            problemDetails.Status = StatusCodes.Status401Unauthorized;
                            problemDetails.Detail = securityTokenExpiredException.Message;

                        }
                        else if (execeptionCatched is SecurityTokenInvalidSignatureException securityTokenInvalid)
                        {
                            problemDetails.Title = "Token inválido";
                            problemDetails.Status = StatusCodes.Status401Unauthorized;
                            problemDetails.Detail = securityTokenInvalid.Message;

                        }
                        else
                        {
                            string message = execeptionCatched.Message;
                            var exception = execeptionCatched.InnerException;

                            while (exception != null)
                            {
                                message += string.Format("{0}\r\n", exception.Message);
                                exception = exception.InnerException;
                            }

                            problemDetails.Title = execeptionCatched.Message;
                            problemDetails.Status = StatusCodes.Status500InternalServerError;
                            problemDetails.Detail = message;
                        }

                        context.Response.StatusCode = problemDetails.Status.Value;
                        context.Response.ContentType = "application/problem+json";

                        var json = JsonConvert.SerializeObject(problemDetails, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        await context.Response.WriteAsync(json);
                    }
                });
            });
        }
        public static IServiceCollection ConfigureProblemDetailsModelState(this IServiceCollection services)
        {
            return services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Consulte as propriedades dos erros para obter detalhes adicionais"
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });
        }
    }
}
