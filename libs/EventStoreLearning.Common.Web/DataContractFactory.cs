using AutoMapper;

namespace EventStoreLearning.Common.Web
{
    public class DataContractFactory : IDataContractFactory
    {
        private readonly IMapper _mapper;

        public DataContractFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public DataContract<TRequest, TResponse> CreateContract<TRequest, TResponse>()
        {
            return new DataContract<TRequest, TResponse>(_mapper);
        }

        public DataContract<TResponse> CreateContract<TResponse>()
        {
            return new DataContract<TResponse>(_mapper);
        }
    }
}
