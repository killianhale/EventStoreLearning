using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Common.EventSourcing;
using Microsoft.AspNetCore.Mvc;

namespace EventStoreLearning.Common.Web
{
    public abstract class BaseCommandController : Controller
    {
        protected BaseCommandController(ICommandMediator commandMediator)
        {
            CommandMediator = commandMediator;
        }

        protected ICommandMediator CommandMediator { get; }

        [NonAction]
        public async Task<CommandResponse<TCommand>> PublishCommand<TCommand>(TCommand command) where TCommand : Command
        {
            CommandResponse<TCommand> response;

            using (var tokenSource = new CancellationTokenSource())
            {
                response = await CommandMediator.PublishCommand(command, tokenSource.Token);
            }

            return response;
        }
    }
}
