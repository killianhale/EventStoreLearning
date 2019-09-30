using System;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModels.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventStoreLearning.Appointment.ReadModel.Repositories
{
    public class ProjectionInfoRepository : IProjectionInfoRepository
    {
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
                    Version = -1
                };
            }

            return info;
        }

        public async Task UpdateProjectionInfo(ProjectionReadModel model)
        {
            var collection = _db.GetCollection<ProjectionReadModel>("readmodel");

            var filter = new BsonDocument();
            var options = new UpdateOptions { IsUpsert = true };

            await collection.ReplaceOneAsync(filter, model, options);
        }
    }
}
