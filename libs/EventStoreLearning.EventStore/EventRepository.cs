using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStoreLearning.Common.EventSourcing;
using EventStoreLearning.Common.Logging;
using Newtonsoft.Json;
using NLog;

namespace EventStoreLearning.EventStore
{
    public class EventRepository : IEventRepository
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IEventStoreClient _store;

        protected IDictionary<string, Func<string, Event>> _eventDeserializers;

        public EventRepository(
            IEventStoreClient store,
            IDictionary<string, Func<string, Event>> eventDeserializers = null
        )
        {
            _store = store;
            _eventDeserializers = eventDeserializers ?? new Dictionary<string, Func<string, Event>>();
        }

        #region Read...

        private async Task<List<Event>> GetAllEventsFromStream(string streamName, long start = StreamPosition.Start)
        {
            var results = new List<Event>();

            StreamEventsSlice currentSlice;

            long nextSliceStart = start;
            var sliceSize = 200;

            using (var connection = _store.Connect())
            {
                do
                {
                    _logger.Debug($"Getting event slice {nextSliceStart}-{sliceSize} from stream '{streamName}' from the event store.");

                    currentSlice = await connection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, sliceSize, true);

                    nextSliceStart = currentSlice.NextEventNumber;

                    var items = currentSlice.Events.Select(e =>
                    {
                        var json = Encoding.ASCII.GetString(e.Event.Data);

                        if (!_eventDeserializers.ContainsKey(e.Event.EventType))
                        {
                            throw new InvalidOperationException($"No event serializer registered for event of type {e.Event.EventType}");
                        }

                        var obj = _eventDeserializers[e.Event.EventType](json);

                        var metaDataJson = Encoding.ASCII.GetString(e.Event.Metadata);
                        var metaDataObj = JsonConvert.DeserializeObject(metaDataJson);
                        var metaData = EventMetadata.FromObject(metaDataObj);

                        _logger.DebugWithContext($"Deserializing metadata for event {e.Event.EventId}", metaDataJson);
                        _logger.TraceWithContext($"Deserialized metadata for event {e.Event.EventId}", metaData);

                        var num = e.IsResolved
                            ? e.Link.EventNumber
                            : e.Event.EventNumber;

                        obj.SetVersion(num);
                        obj.SetMetadata(metaData);

                        return obj;
                    });

                    results.AddRange(items);
                } while (!currentSlice.IsEndOfStream);
            }

            return results;
        }

        public async Task<List<Event>> GetAllEventsForAggregateType<T>(long start = StreamPosition.Start) where T : AggregateRoot
        {
            var type = typeof(T);
            var streamName = $"$ce-{type.Name}";

            _logger.Info($"Getting all events for Aggregate of type {type.Name} (Stream '{streamName}') starting at position {start}.");

            var results = await GetAllEventsFromStream(streamName, start);

            return results;
        }

        public async Task<List<Event>> GetAllEventsOfType<T>(long start = StreamPosition.Start) where T : Event
        {
            var type = typeof(T);
            var streamName = $"$et-{type.Name}";

            _logger.Info($"Getting all events for of type {type.Name} (Stream '{streamName}') starting at position {start}.");

            var results = await GetAllEventsFromStream(streamName, start);

            return results;
        }

        public async Task<T> GetAggregateById<T>(Guid id) where T : AggregateRoot, new()
        {
            var type = typeof(T);

            _logger.Info($"Getting Aggregate of type {type.Name} and ID {id}.");

            var result = new T();

            var eventsResponse = await GetAllEventsForAggregateType<T>();
            var events = eventsResponse.Where(e => e.AggregateId == id);

            _logger.Debug($"Filtered to {events.Count()}/{eventsResponse.Count} events relating to Aggregate of type {type.Name} and ID {id}.");

            result.LoadFromHistory(events);

            return result;
        }

        #endregion

        #region Write...

        public async Task<Exception> Save<T>(T aggregate, long expectedVersion = -1) where T : AggregateRoot
        {
            Exception error = null;

            try
            {
                _logger.InfoWithContext($"Saving changes on Aggregate {aggregate.GetType().Name} ({aggregate.Id}).", aggregate);

                using (var connection = _store.Connect())
                {
                    _logger.DebugWithContext($"Writing changes for Aggregate {aggregate.GetType().Name} ({aggregate.Id}) to the event store.", aggregate);

                    var data = aggregate.GetUncommittedChanges().Select(e =>
                    {
                        var meta = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(e.GetMetadata()));
                        var body = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(e));

                        return new EventData(e.GetMetadata().Id, e.GetType().Name, true, body, meta);
                    });

                    var result = await connection.AppendToStreamAsync($"{aggregate.GetType().Name}-{aggregate.Id}", expectedVersion, data);
                }

                aggregate.MarkChangesAsCommitted();
            }
            catch (Exception ex)
            {
                error = ex;

                _logger.WarnWithContext($"An error occured while trying to save Aggregate {aggregate.GetType().Name} ({aggregate.Id})",
                    new { error, aggregate });
            }

            return error;
        }

        #endregion
    }
}
