using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EventStoreLearning.Mongo
{
    public static class MongoContainerBuilderExtenstions
    {
        public static void ConfigureMongo(this ContainerBuilder builder, string configName = null)
        {
            builder.Register<IMongoDatabase>(context =>
            {
                MongoDbConfig config = null;

                if(string.IsNullOrEmpty(configName))
                {
                    var configAccesor = context.Resolve<IOptions<MongoDbConfig>>();
                    config = configAccesor.Value;
                } else
                {
                    var configAccesor = context.Resolve<IOptionsSnapshot<MongoDbConfig>>();
                    config = configAccesor.Get(configName);
                }

                if(config == null)
                {
                    throw new InvalidOperationException("No MongoDB config found!");
                }

                if(string.IsNullOrEmpty(config.ConnectionString) || string.IsNullOrEmpty(config.DatabaseName))
                {
                    throw new InvalidOperationException("Invalid MongoDB config!");
                }

                var client = new MongoClient(config.ConnectionString);
                var database = client.GetDatabase(config.DatabaseName);

                return database;
            });
        }
    }
}
