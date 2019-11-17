using System;
using System.Threading.Tasks;
using EventStoreLearning.Common.Web.Models;
using EventStoreLearning.Common.EventSourcing;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace EventStoreLearning.Common.Web.Extensions
{
    public static class IResponseExtensions
    {
        public async static Task<JsonResult> AsApiResponse<TRequest, TResponse>(
            this Task<IResponse<TRequest, TResponse>> response,
            int successStatusCode = 200,
            int errorStatusCode = 400
            )
        {
            var r = await response;

            return AsApiResponse(r, successStatusCode, errorStatusCode);
        }

        public async static Task<JsonResult> AsApiResponse<TRequest, TResponse>(
            this Task<IResponse<TRequest, TResponse>> response,
            object successBody,
            int successStatusCode = 200,
            int errorStatusCode = 400
            )
        {
            var r = await response;

            return AsApiResponse(r, successBody, successStatusCode, errorStatusCode);
        }

        public static JsonResult AsApiResponse<TRequest, TResponse>(
            this IResponse<TRequest, TResponse> response,
            int successStatusCode = 200,
            int errorStatusCode = 400
            )
        {
            var defaultSuccessBody = new { Message = "Success!" };

            return AsApiResponse(response, defaultSuccessBody, successStatusCode, errorStatusCode);
        }

        public static JsonResult AsApiResponse<TRequest, TResponse>(
            this IResponse<TRequest, TResponse> response,
            object successBody,
            int successStatusCode = 200,
            int errorStatusCode = 400,
            IMapper mapper = null
            )
        {
            return response.Error == null
                   ? new JsonResult(successBody) { StatusCode = successStatusCode }
                   : new JsonResult(new ErrorResponse(response.Error.Message, response.Error.StackTrace)) { StatusCode = errorStatusCode };
        }

        public async static Task<JsonResult> AsApiResponse<TRequest, TResponse, TMappedResponse>(
            this Task<IResponse<TRequest, TResponse>> response,
            int successStatusCode = 200,
            int errorStatusCode = 400,
            IMapper mapper = null
            )
        {
            var r = await response;

            return AsApiResponse<TRequest, TResponse, TMappedResponse>(r, successStatusCode, errorStatusCode, mapper);
        }

        public static JsonResult AsApiResponse<TRequest, TResponse, TMappedResponse>(
            this IResponse<TRequest, TResponse> response,
            int successStatusCode = 200,
            int errorStatusCode = 400,
            IMapper mapper = null
            )
        {
            var successBody = (mapper != null)
                ? (object)mapper.Map<TMappedResponse>(response.Response)
                : response.Response;

            return response.Error == null
                   ? new JsonResult(successBody) { StatusCode = successStatusCode }
                   : new JsonResult(new ErrorResponse(response.Error.Message, response.Error.StackTrace)) { StatusCode = errorStatusCode };
        }
    }
}
