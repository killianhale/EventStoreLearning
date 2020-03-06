using System;
using MediatR;

namespace EventStoreLearning.EventSourcing
{
    public interface IMessage<TIdentity> : IRequest<TIdentity>
    {
    }
}
