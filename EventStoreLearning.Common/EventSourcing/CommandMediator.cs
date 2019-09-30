using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventStoreLearning.Common.EventSourcing
{
    public class CommandMediator : ICommandMediator
    {
        private readonly IMediator _mediator;

        public CommandMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CommandResponse<TCommand>> PublishCommand<TCommand>(TCommand command, CancellationToken cancelationToken)
            where TCommand : Command
        {
            var request = new CommandRequest<TCommand>(command);

            return await _mediator.Send(request, cancelationToken);
        }
    }
}
