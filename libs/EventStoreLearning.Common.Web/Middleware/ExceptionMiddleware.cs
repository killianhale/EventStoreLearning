using System;
using System.Net;
using System.Threading.Tasks;
using EventStoreLearning.Common.Web.Models;
using ContextRunner;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ReasonCodeExceptions;
using AggregateOP.Exceptions;

namespace EventStoreLearning.Common.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                if(httpContext.Response.HasStarted)
                {
                    throw;
                }

                var statusCode = HttpStatusCode.InternalServerError;
                var reasonCode = ex is ReasonCodeException reasonEx ? reasonEx.ReasonCode : "?";
                var message = ex.Message;
                var stackTrace = ex.StackTrace;

                if (ex is ServiceTimeoutException || ex is TimeoutException)
                {
                    statusCode = HttpStatusCode.GatewayTimeout;
                }
                else if (ex is EnvironmentException)
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                else if (ex is DataNotFoundException
                    || ex is IAggregateDependencyException
                    || ex is IAggregateNotFoundException)
                {
                    statusCode = HttpStatusCode.NotFound;
                }
                else if (ex is DataConflictException || ex is IAggregateConflictException)
                {
                    statusCode = HttpStatusCode.Conflict;
                }
                else if (ex is MiscDataException
                    || ex is ArgumentException
                    || ex is IAggregateRootException)
                {
                    statusCode = HttpStatusCode.BadRequest;
                }
                else if (ex is DataException)
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                else if (ex is PermissionsException)
                {
                    statusCode = HttpStatusCode.Forbidden;
                }
                else if (ex is AuthenticationException)
                {
                    statusCode = HttpStatusCode.Unauthorized;
                }

                httpContext.Response.Clear();
                httpContext.Response.StatusCode = (int)statusCode;
                httpContext.Response.ContentType = "application/json";

                var body = new ErrorResponse(message, reasonCode, stackTrace);

                var responseStr = JsonConvert.SerializeObject(body,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Formatting = Formatting.Indented
                    });

                
                await httpContext.Response.WriteAsync(responseStr);
                await httpContext.Response.Body.FlushAsync();
            }
        }
    }
}
