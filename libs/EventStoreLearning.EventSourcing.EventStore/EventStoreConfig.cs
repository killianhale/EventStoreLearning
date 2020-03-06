using System;
namespace EventStoreLearning.EventSourcing.EventStore
{
    public class EventStoreConfig
    {
        public string ApplicationName { get; set; }
        public string ConnectionString { get; set; }
    }
}
