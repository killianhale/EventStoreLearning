using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace EventStoreLearning.Common.Web
{
    public class MediatedDataContract<TRequest, TResponse> : DataContract<TRequest, TResponse>
    {
        private readonly IMediator _mediator;

        public MediatedDataContract(IMediator mediator, IMapper mapper) : base(mapper)
        {
            _mediator = mediator;
        }

        public Func<TRequest, Task<TResponse>> Mediate<TInput, TOutput>()
            where TInput : IRequest<TOutput>
        {
            return Wrap<TInput, TOutput>(async i =>
            {
                TOutput result;

                using (var tokenSource = new CancellationTokenSource())
                {
                    result = await _mediator.Send(i, tokenSource.Token);
                }

                return result;
            });
        }

        public Func<TRequest, Task<TResponse>> Mediate<TInput>()
            where TInput : IRequest<TResponse>
        {
            return Wrap<TInput>(async i =>
            {
                TResponse result;

                using (var tokenSource = new CancellationTokenSource())
                {
                    result = await _mediator.Send(i, tokenSource.Token);
                }

                return result;
            });
        }
    }

    public class MediatedDataContract<TResponse> : DataContract<TResponse>
    {
        private readonly IMediator _mediator;

        public MediatedDataContract(IMediator mediator, IMapper mapper) : base(mapper)
        {
            _mediator = mediator;
        }

        public Func<Task<TResponse>> Mediate<TInput, TOutput>()
            where TInput : IRequest<TOutput>, new()
        {
            return Wrap<TInput, TOutput>(async i =>
            {
                TOutput result;

                using (var tokenSource = new CancellationTokenSource())
                {
                    result = await _mediator.Send(i, tokenSource.Token);
                }

                return result;
            });
        }

        public Func<Task<TResponse>> Mediate<TInput>()
            where TInput : IRequest<TResponse>, new()
        {
            return Wrap<TInput, TResponse>(async i =>
            {
                TResponse result;

                using (var tokenSource = new CancellationTokenSource())
                {
                    result = await _mediator.Send(i, tokenSource.Token);
                }

                return result;
            });
        }
    }
}
