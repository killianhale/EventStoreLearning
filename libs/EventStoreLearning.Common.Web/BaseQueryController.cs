using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Common.Querying;
using Microsoft.AspNetCore.Mvc;

namespace EventStoreLearning.Common.Web
{
    public abstract class BaseQueryController : Controller
    {
        protected BaseQueryController(IQuery queryMediator)
        {
            QueryMediator = queryMediator;
        }

        protected IQuery QueryMediator { get; }

        [NonAction]
        public async Task<QueryResponse<TQuery, TResult>> PublishQuery<TQuery, TResult>(TQuery query)
        {
            QueryResponse<TQuery, TResult> response;

            using (var tokenSource = new CancellationTokenSource())
            {
                response = await QueryMediator.Query<TQuery, TResult>(query, tokenSource.Token);
            }

            return response;
        }
    }
}
