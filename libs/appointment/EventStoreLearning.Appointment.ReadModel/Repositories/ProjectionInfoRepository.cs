using System;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModels.Models;
using EventStoreLearning.Common.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using NLog;

namespace EventStoreLearning.Appointment.ReadModel.Repositories
{
    public class ProjectionInfoRepository : IProjectionInfoRepository
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMongoDatabase _db;

        public ProjectionInfoRepository(IMongoDatabase db)
        {
            _db = db;
        }

        public async Task<ProjectionReadModel> RetrieveProjectionInfo()
        {
            var collection = _db.GetCollection<ProjectionReadModel>("readmodel");

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

            _logger.DebugWithContext("Retrieved projection info from DB", info);

            return info;
        }

        public async Task UpdateProjectionInfo(ProjectionReadModel model)
        {
            _logger.DebugWithContext("Updating projection info", model);

            var collection = _db.GetCollection<ProjectionReadModel>("readmodel");

            var filter = new BsonDocument();
            var options = new UpdateOptions { IsUpsert = true };

            await collection.ReplaceOneAsync(filter, model, options);
        }
    }
}
