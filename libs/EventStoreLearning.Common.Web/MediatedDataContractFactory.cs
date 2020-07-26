using AutoMapper;
using MediatR;

namespace EventStoreLearning.Common.Web
{
    public class MediatedDataContractFactory : IMediatedDataContractFactory
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MediatedDataContractFactory(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public MediatedDataContract<TRequest, TResponse> CreateContract<TRequest, TResponse>()
        {
            return new MediatedDataContract<TRequest, TResponse>(_mediator, _mapper);
        }

        public MediatedDataContract<TResponse> CreateContract<TResponse>()
        {
            return new MediatedDataContract<TResponse>(_mediator, _mapper);
        }
    }
}
