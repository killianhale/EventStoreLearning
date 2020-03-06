using System;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModels.Models;
using ContextRunner.Base;
using EventStoreLearning.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventStoreLearning.Appointment.ReadModel.Repositories
{
    public class ProjectionInfoRepository : IProjectionInfoRepository
    {
        private readonly IMongoDocumentClient _mongo;

        public ProjectionInfoRepository(IMongoDocumentClient mongo)
        {
            _mongo = mongo;
        }

        public async Task<ProjectionReadModel> RetrieveProjectionInfo()
        {
            return await _mongo.ConnectWithContext(async (IMongoDatabase db, ActionContext context) =>
            {
                context.Logger.Debug("Retrieving projection info from DB");

                var collection = db.GetCollection<ProjectionReadModel>("readmodel");

                ProjectionReadModel info = null;

                var filter = new BsonDocument();
                using (var cursor = await collection.FindAsync(filter))
                {
                    info = await cursor.FirstOrDefaultAsync();
                }

                if (info == null)
                {
                    info = new ProjectionReadModel()
                    {
                        Id = Guid.NewGuid(),
                        Version = -1
                    };
                }

                context.State.SetParam("projectionInfo", info);

                return info;
            });
        }

        public async Task UpdateProjectionInfo(ProjectionReadModel model)
        {
            await _mongo.ConnectWithContext(async (IMongoDatabase db, ActionContext context) =>
            {
                context.Logger.Debug("Updating projection info in the DB");

                context.State.SetParam("projectionInfo", model);

                var collection = db.GetCollection<ProjectionReadModel>("readmodel");

                var filter = new BsonDocument();
                var options = new ReplaceOptions { IsUpsert = true };

                await collection.ReplaceOneAsync(filter, model, options);
            });
        }
    }
}
