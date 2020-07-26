namespace EventStoreLearning.Common.Web
{
    public interface IMediatedDataContractFactory
    {
        MediatedDataContract<TRequest, TResponse> CreateContract<TRequest, TResponse>();
        MediatedDataContract<TResponse> CreateContract<TResponse>();
    }
}