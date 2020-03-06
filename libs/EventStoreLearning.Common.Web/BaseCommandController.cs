using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.EventSourcing.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventStoreLearning.Common.Web
{
    public abstract class BaseCommandController : Controller
    {
        protected BaseCommandController(IMediator mediator)
        {
            Mediator = mediator;
        }

        protected IMediator Mediator { get; }

        [NonAction]
        public async Task<Guid> PublishCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            var response = Guid.Empty;

            using (var tokenSource = new CancellationTokenSource())
            {
                response = await Mediator.Send(command, tokenSource.Token);
            }

            return response;
        }

        [NonAction]
        public JsonResult CreateApiResponse(object successBody, int responseCode = StatusCodes.Status200OK)
        {
            var response = new JsonResult(successBody) { StatusCode = responseCode };

            return response;
        }
    }
}
