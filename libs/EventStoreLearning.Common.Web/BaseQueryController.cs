using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventStoreLearning.Common.Querying;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventStoreLearning.Common.Web
{
    public abstract class BaseQueryController : Controller
    {
        protected BaseQueryController(IMediate queryMediator)
        {
            QueryMediator = queryMediator;
        }

        protected IMediate QueryMediator { get; }

        [NonAction]
        public async Task<MediationResponse<TQuery, TResult>> PublishQuery<TQuery, TResult>(TQuery query)
        {
            MediationResponse<TQuery, TResult> response;

            using (var tokenSource = new CancellationTokenSource())
            {
                response = await QueryMediator.Mediate<TQuery, TResult>(query, tokenSource.Token);
            }

            return response;
        }

        [NonAction]
        public JsonResult CreateApiResponse(object successBody, int responseCode = StatusCodes.Status200OK)
        {
            var response = new JsonResult(successBody) { StatusCode = responseCode };

            return response;
        }

        [NonAction]
        public JsonResult CreateApiResponse<TContent, TResponse>(TContent content, int responseCode = StatusCodes.Status200OK, IMapper mapper = null)
        {
            var body = (mapper != null)
                ? (object)mapper.Map<TResponse>(content)
                : content;

            var response = new JsonResult(body) { StatusCode = responseCode };

            return response;
        }
    }
}
