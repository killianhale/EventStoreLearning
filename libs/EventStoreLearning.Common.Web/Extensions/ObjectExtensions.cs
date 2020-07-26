using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventStoreLearning.Common.Web.Extensions
{
    public static class ObjectExtensions
    {
        //public static JsonResult CreateApiResponse<T>(
        //    this T successBody,
        //    int responseCode = StatusCodes.Status200OK
        //    ) where T : class
        //{
        //    var response = new JsonResult(successBody) { StatusCode = responseCode };

        //    return response;
        //}

        public static async Task<JsonResult> CreateApiResponse<T>(
            this Task<T> successBodyTask,
            int responseCode = StatusCodes.Status200OK)
        {
            var successBody = await successBodyTask;

            var response = new JsonResult(successBody) { StatusCode = responseCode };

            return response;
        }
    }
}
