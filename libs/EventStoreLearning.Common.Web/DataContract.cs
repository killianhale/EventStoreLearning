using System;
using System.Threading.Tasks;
using AutoMapper;

namespace EventStoreLearning.Common.Web
{
    public class DataContract<TRequest, TResponse>
    {
        private readonly IMapper _mapper;

        public DataContract(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Func<TRequest, Task<TResponse>> Wrap<TInput, TOutput>(Func<TInput, Task<TOutput>> action)
        {
            async Task<TResponse> result(TRequest request)
            {
                var domainObj = _mapper.Map<TInput>(request);

                var output = await action?.Invoke(domainObj);

                var response = _mapper.Map<TResponse>(output);

                return response;
            }

            return result;
        }

        public Func<TRequest, Task<TResponse>> Wrap<TInput>(Func<TInput, Task<TResponse>> action)
        {
            async Task<TResponse> result(TRequest request)
            {
                var domainObj = _mapper.Map<TInput>(request);

                var output = await action?.Invoke(domainObj);

                return output;
            }

            return result;
        }

        public Func<TRequest, Task<TResponse>> Wrap<TOutput>(Func<TRequest, Task<TOutput>> action)
        {
            async Task<TResponse> result(TRequest request)
            {
                var output = await action?.Invoke(request);

                var response = _mapper.Map<TResponse>(output);

                return response;
            }

            return result;
        }
    }

    public class DataContract<TResponse>
    {
        private readonly IMapper _mapper;

        public DataContract(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Func<Task<TResponse>> Wrap<TInput, TOutput>(Func<TInput, Task<TOutput>> action)
            where TInput : new()
        {
            async Task<TResponse> result()
            {
                var domainObj = new TInput();

                var output = await action?.Invoke(domainObj);

                var response = _mapper.Map<TResponse>(output);

                return response;
            }

            return result;
        }

        public Func<Task<TResponse>> Wrap<TInput>(Func<TInput, Task<TResponse>> action)
            where TInput : new()
        {
            async Task<TResponse> result()
            {
                var domainObj = new TInput();

                var output = await action?.Invoke(domainObj);

                return output;
            }

            return result;
        }
    }
}
