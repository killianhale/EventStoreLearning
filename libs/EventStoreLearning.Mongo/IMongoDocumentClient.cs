using System;
using System.Threading.Tasks;
using ContextRunner.Base;
using MongoDB.Driver;

namespace EventStoreLearning.Mongo
{
    public interface IMongoDocumentClient
    {
        Task<T> ConnectWithContext<T>(Func<IMongoDatabase, ActionContext, Task<T>> action);
        Task ConnectWithContext(Func<IMongoDatabase, ActionContext, Task> action);
    }
}