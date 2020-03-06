using System;
using System.Linq;
using System.Threading.Tasks;
using ContextRunner;
using ContextRunner.Base;
using EventStoreLearning.Exceptions;
using MongoDB.Driver;

namespace EventStoreLearning.Mongo
{
    public class MongoDocumentClient : IMongoDocumentClient
    {
        private readonly IContextRunner _runner;
        private readonly MongoDbConfig _config;

        public MongoDocumentClient(MongoDbConfig mongoOptions) : this(null, mongoOptions) { }

        public MongoDocumentClient(IContextRunner runner, MongoDbConfig mongoOptions)
        {
            _runner = runner ?? new ContextRunner.ContextRunner();
            _config = mongoOptions;
        }

        public async Task ConnectWithContext(Func<IMongoDatabase, ActionContext, Task> action)
        {
            await ConnectWithContext<object>(async (IMongoDatabase db, ActionContext context) =>
            {
                await action(db, context);

                return null;
            });
        }

        public async Task<T> ConnectWithContext<T>(Func<IMongoDatabase, ActionContext, Task<T>> action)
        {
            return await _runner.RunAction(async context =>
            {
                try
                {
                    context.State.SetParam("mongoConnection", SanitizedConnectionString);

                    var db = Connect();
                    return await action(db, context);
                }
                catch (MongoClientException ex)
                {
                    throw LogAndReturnException(context.Logger.Error, new InternalServiceException("MongoDB", ex));
                }
                catch (MongoInternalException ex)
                {
                    throw LogAndReturnException(context.Logger.Error, new InternalServiceException("MongoDB", ex));
                }
                catch (MongoConnectionException ex) when (ex is MongoAuthenticationException)
                {
                    throw LogAndReturnException(context.Logger.Error, new ServiceAuthenticationException("MongoDB", ex));
                }
                catch (MongoConnectionException ex)
                {
                    throw LogAndReturnException(context.Logger.Error, new ServiceConnectionException("MongoDB", ex));
                }
                catch (TimeoutException ex)
                {
                    throw LogAndReturnException(context.Logger.Error, new ServiceConnectionException("MongoDB", ex));
                }
                catch (MongoServerException ex) when (ex is MongoCommandException)
                {
                    throw LogAndReturnException(context.Logger.Error, new ServiceStateException("MongoDB", ex));
                }
                catch (MongoServerException ex) when (ex is MongoExecutionTimeoutException)
                {
                    throw LogAndReturnException(context.Logger.Error, new ServiceTimeoutException("MongoDB", ex));
                }
                catch (MongoServerException ex) when (ex is MongoWriteConcernException wcEx)
                {
                    //TODO: Parse the codes to know what the problem is...
                    throw LogAndReturnException(context.Logger.Debug, new DataConflictException("Stale data! Unable to write to stream.", ex));
                }
                catch (MongoServerException ex) when (ex is MongoBulkWriteException)
                {
                    //TODO: Parse the codes to know what the problem is...
                    throw LogAndReturnException(context.Logger.Debug, new DataNotFoundException("The requested data was not found.", ex));
                }
                catch (MongoServerException ex) when (ex is MongoQueryException)
                {
                    throw LogAndReturnException(context.Logger.Debug, new DataNotFoundException("The requested data was not found.", ex));
                }
                catch (MongoException ex)
                {
                    throw LogAndReturnException(context.Logger.Error, new InternalServiceException("MongoDB", ex));
                }
                catch (Exception ex)
                {
                    throw LogAndReturnException(context.Logger.Error, ex);
                }
            }, nameof(MongoDocumentClient));
        }

        private IMongoDatabase Connect()
        {
            if (_config == null)
            {
                throw new InvalidOperationException("No MongoDB config found!");
            }

            if (string.IsNullOrEmpty(_config.ConnectionString))
            {
                throw new InvalidOperationException("Invalid MongoDB config!");
            }

            var client = new MongoClient(_config.ConnectionString);
            var database = client.GetDatabase(_config.DatabaseName);

            return database;
        }

        private string SanitizedConnectionString
        {
            get
            {
                var connString = _config?.ConnectionString;

                if (connString == null || connString.IndexOf("@") < 0 || connString.Count(c => c == ':') < 2)
                {
                    return connString;
                }

                var parts = connString.Split("@", StringSplitOptions.None);
                parts[0] = parts[0].Substring(0, parts[0].LastIndexOf(':'));

                var sanitized = string.Join('@', parts);

                return sanitized;
            }
        }

        private Exception LogAndReturnException(Action<string> logMethod, Exception ex)
        {
            logMethod?.Invoke(ex.Message);

            return ex;
        }
    }
}
