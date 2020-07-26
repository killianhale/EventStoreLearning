using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace EventStoreLearning.Appointment.ReadModels.Models
{
    public class AppointmentReadModel
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public long Version { get; set; }
        public string Title { get; set;  }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
    }
}
