using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventStoreLearning.Common.Querying
{
    public interface IQuery
    {
        Task<QueryResponse<TQuery, TResult>> Query<TQuery, TResult>(TQuery query, CancellationToken cancelationToken);
    }
}