using System;
using System.Net;
using System.Threading.Tasks;
using EventStoreLearning.Common.Web.Models;
using ContextRunner;
using EventStoreLearning.EventSourcing.Exceptions;
using EventStoreLearning.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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

                var lookupException = ex is ContextException ? ex.InnerException : ex;

                var statusCode = HttpStatusCode.InternalServerError;
                var reasonCode = lookupException is ReasonCodeException reasonEx ? reasonEx.ReasonCode : "?";
                var message = lookupException.Message;
                var stackTrace = lookupException.StackTrace;

                if (lookupException is ServiceTimeoutException || lookupException is TimeoutException)
                {
                    statusCode = HttpStatusCode.GatewayTimeout;
                }
                else if (lookupException is EnvironmentException)
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                else if (lookupException is DataNotFoundException
                    || lookupException is IAggregateDependencyException
                    || lookupException is IAggregateNotFoundException)
                {
                    statusCode = HttpStatusCode.NotFound;
                }
                else if (lookupException is DataConflictException || lookupException is IAggregateConflictException)
                {
                    statusCode = HttpStatusCode.Conflict;
                }
                else if (lookupException is MiscDataException
                    || lookupException is ArgumentException
                    || lookupException is IAggregateRootException)
                {
                    statusCode = HttpStatusCode.BadRequest;
                }
                else if (lookupException is DataException)
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }
                else if (lookupException is PermissionsException)
                {
                    statusCode = HttpStatusCode.Forbidden;
                }
                else if (lookupException is AuthenticationException)
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
