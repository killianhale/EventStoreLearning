namespace EventStoreLearning.Common.Web
{
    public interface IDataContractFactory
    {
        DataContract<TRequest, TResponse> CreateContract<TRequest, TResponse>();
        DataContract<TResponse> CreateContract<TResponse>();
    }
}