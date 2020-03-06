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
            builder.Register<MongoDbConfig>(context =>
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

                return config;
            });

            builder.RegisterType<MongoDocumentClient>()
                .As<IMongoDocumentClient>();
        }
    }
}
