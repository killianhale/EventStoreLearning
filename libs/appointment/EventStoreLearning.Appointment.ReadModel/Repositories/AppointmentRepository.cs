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
                context.Logger.Debug("Creating new appointment inside the DB");

                context.State.SetParam("appointment", model);

                var collection = db.GetCollection<AppointmentReadModel>("appointments");

                await collection.InsertOneAsync(model);
            });
        }

        public async Task ReplaceAppointment(AppointmentReadModel model)
        {
            await _mongo.ConnectWithContext(async (IMongoDatabase db, ActionContext context) =>
            {
                context.Logger.Debug($"Replacing appointment {model.Id} inside the DB");

                context.State.SetParam("appointment", model);

                var collection = db.GetCollection<AppointmentReadModel>("appointments");

                var filter = Builders<AppointmentReadModel>.Filter.Eq(appt => appt.Id, model.Id);

                await collection.ReplaceOneAsync(filter, model);
            });
        }

        public async Task UpdateAppointment(Guid id, long version, string title, DateTime? startTime, TimeSpan? duration)
        {
            await _mongo.ConnectWithContext(async (IMongoDatabase db, ActionContext context) =>
            {
                context.Logger.Debug($"Updating appointment ID {id} inside the DB");

                context.State.SetParam("appointmentUpdate", new
                {
                    Id = id,
                    Title = title,
                    StartTime = startTime,
                    Duration = duration
                });

                var collection = db.GetCollection<AppointmentReadModel>("appointments");

                var changes = new List<UpdateDefinition<AppointmentReadModel>>()
                {
                    Builders<AppointmentReadModel>.Update.Set(appt => appt.Version, version)
                };

                if (title != null)
                {
                    changes.Add(Builders<AppointmentReadModel>.Update.Set(appt => appt.Title, title));
                }

                if (startTime.HasValue)
                {
                    changes.Add(Builders<AppointmentReadModel>.Update.Set(appt => appt.StartTime, startTime.Value));
                }

                if (duration.HasValue)
                {
                    changes.Add(Builders<AppointmentReadModel>.Update.Set(appt => appt.Duration, duration.Value.TotalMinutes));
                }

                var filter = Builders<AppointmentReadModel>.Filter.Eq(appt => appt.Id, id);
                var update = Builders<AppointmentReadModel>.Update.Combine(changes);

                await collection.UpdateOneAsync(filter, update);
            });
        }

        public async Task DeleteAppointment(Guid id)
        {
            await _mongo.ConnectWithContext(async (IMongoDatabase db, ActionContext context) =>
            {
                context.Logger.Debug($"Deleting appointment ID {id} from the DB");

                var collection = db.GetCollection<AppointmentReadModel>("appointments");

                var filter = Builders<AppointmentReadModel>.Filter.Eq(appt => appt.Id, id);

                await collection.DeleteOneAsync(filter);
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
