using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModels.Models;
using EventStoreLearning.Common.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using NLog;

namespace EventStoreLearning.Appointment.ReadModel.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMongoDatabase _db;

        public AppointmentRepository(IMongoDatabase db)
        {
            _db = db;
        }

        public async Task CreateAppointment(AppointmentReadModel model)
        {
            _logger.DebugWithContext("Adding new appointment to the DB", model);

            var collection = _db.GetCollection<AppointmentReadModel>("appointments");

            await collection.InsertOneAsync(model);
        }

        public async Task<IList<AppointmentReadModel>> RetrieveAllAppointments()
        {
            _logger.Debug("Retrieving all appointments from DB...");

            var collection = _db.GetCollection<AppointmentReadModel>("appointments");
            var filter = new BsonDocument();

            var result = new List<AppointmentReadModel>();

            using (var cursor = await collection.FindAsync(filter))
            {
                result = await cursor.ToListAsync();
            }

            _logger.Debug($"{result.Count} appointments found.");

            return result;
        }
    }
}
