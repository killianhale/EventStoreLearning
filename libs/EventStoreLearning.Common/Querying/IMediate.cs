using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventStoreLearning.Common.Querying
{
    public interface IMediate
    {
        Task<MediationResponse<TQuery, TResult>> Mediate<TQuery, TResult>(TQuery query, CancellationToken cancelationToken);
    }
}