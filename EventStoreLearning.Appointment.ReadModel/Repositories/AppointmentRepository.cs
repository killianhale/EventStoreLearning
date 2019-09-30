using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModels.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventStoreLearning.Appointment.ReadModel.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IMongoDatabase _db;

        public AppointmentRepository(IMongoDatabase db)
        {
            _db = db;
        }

        public async Task CreateAppointment(AppointmentReadModel model)
        {
            var collection = _db.GetCollection<AppointmentReadModel>("appointments");

            await collection.InsertOneAsync(model);
        }

        public async Task<IList<AppointmentReadModel>> RetrieveAllAppointments()
        {
            var collection = _db.GetCollection<AppointmentReadModel>("appointments");
            var filter = new BsonDocument();

            var result = new List<AppointmentReadModel>();

            using (var cursor = await collection.FindAsync(filter))
            {
                result = await cursor.ToListAsync();
            }

            return result;
        }
    }
}
