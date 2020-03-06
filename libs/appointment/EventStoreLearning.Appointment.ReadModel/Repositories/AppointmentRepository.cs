using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModels.Models;
using ContextRunner.Base;
using EventStoreLearning.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventStoreLearning.Appointment.ReadModel.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IMongoDocumentClient _mongo;

        public AppointmentRepository(IMongoDocumentClient mongo)
        {
            _mongo = mongo;
        }

        public async Task CreateAppointment(AppointmentReadModel model)
        {
            await _mongo.ConnectWithContext(async (IMongoDatabase db, ActionContext context) =>
            {
                context.Logger.Debug("Adding new appointment to the DB");

                context.State.SetParam("appointment", model);

                var collection = db.GetCollection<AppointmentReadModel>("appointments");

                await collection.InsertOneAsync(model);
            });
        }

        public async Task<IList<AppointmentReadModel>> RetrieveAllAppointments()
        {
            return await _mongo.ConnectWithContext(async (IMongoDatabase db, ActionContext context) =>
            {
                context.Logger.Debug("Retrieving all appointments from DB");

                var collection = db.GetCollection<AppointmentReadModel>("appointments");
                var filter = new BsonDocument();

                var result = new List<AppointmentReadModel>();

                using (var cursor = await collection.FindAsync(filter))
                {
                    result = await cursor.ToListAsync();
                }

                context.Logger.Debug($"{result.Count} appointments found.");

                context.State.SetParam("appointments", result);

                return result;
            });
        }
    }
}
