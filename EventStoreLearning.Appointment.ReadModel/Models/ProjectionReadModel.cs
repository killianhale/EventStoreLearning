using System;
using MongoDB.Bson;

namespace EventStoreLearning.Appointment.ReadModels.Models
{
    public class ProjectionReadModel
    {
        public ObjectId _id;
        public long Version;
    }
}
