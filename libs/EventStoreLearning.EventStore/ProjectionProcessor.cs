using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Common.EventSourcing;
using EventStoreLearning.Common.Logging;
using NLog;

namespace EventStoreLearning.EventStore
{
    public abstract class ProjectionProcessor<T> : IProjectionProcessor where T : AggregateRoot
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IEventRepository _repo;
        private readonly IEventMediator _eventMediator;

        private readonly ConcurrentQueue<Event> _queue;
        private readonly AutoResetEvent _killEvent;

        private long position;
        private CancellationTokenSource cancellationToken;

        private Thread _queueThread;
        private Thread _processThread;

        private Exception error;

        protected ProjectionProcessor(IEventRepository repo, IEventMediator eventMediator)
        {
            _repo = repo;
            _eventMediator = eventMediator;

            _queue = new ConcurrentQueue<Event>();
            _killEvent = new AutoResetEvent(false);
        }

        private void Run()
        {
            _queueThread = new Thread(new ThreadStart(Enqueue))
            {
                IsBackground = true
            };

            _processThread = new Thread(new ThreadStart(ProcessQueue))
            {
                IsBackground = true,

            };

            _queueThread.Start();
            _processThread.Start();

            _killEvent.WaitOne();

            Stop();

            if (error != null)
            {
                throw error;
            }
        }

        private void Enqueue()
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Fetching events for aggregate type '{typeof(T).Name}' at position {position}...");

                    var events = _repo.GetAllEventsForAggregateType<T>(position).Result;

                    if (events.Any())
                    {
                        _logger.Debug($"Adding {events.Count} events to projection processor queue for aggregate type '{typeof(T).Name}'.");
                    }

                    for (var x = 0; x < events.Count; x++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            _logger.Debug($"Cancellation Requested! Breaking out of queue for aggregate type '{typeof(T).Name}'.");

                            break;
                        }

                        var e = events[x];

                        position = e.GetVersion() + 1;

                        _queue.Enqueue(e);
                    }

                    Thread.Sleep(10000);
                }
            }
            catch(Exception ex)
            {
                _logger.WarnWithContext($"An error occured while trying to fetch events for aggregate type '{typeof(T).Name}'!", ex);

                error = ex;
                _killEvent.Set();
            }
        }

        private void ProcessQueue()
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Processing queue for aggregate type '{typeof(T).Name}'...");

                    while (!_queue.IsEmpty && !cancellationToken.IsCancellationRequested)
                    {
                        _logger.Trace($"Attempting to dequeue item for aggregate type '{typeof(T).Name}'...");

                        var success = _queue.TryDequeue(out Event e);

                        if (success)
                        {
                            _logger.TraceWithContext("Processing dequeued event", e);

                            _eventMediator.PublishEvent(e, cancellationToken.Token).Wait();

                            SaveEventPosition(e.GetVersion()).Wait();
                        }
                    }

                    Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                _logger.WarnWithContext($"An error occured while trying to process the queue for aggregate type '{typeof(T).Name}'!", ex);

                error = ex;
                _killEvent.Set();
            }
        }

        protected abstract Task SaveEventPosition(long position);

        protected abstract Task<long> GetStartEventPosition();

        public void Start()
        {
            _logger.Info($"Starting projection processor for aggregate type '{typeof(T).Name}'...");

            position = GetStartEventPosition().Result;

            cancellationToken = new CancellationTokenSource();

            Run();
        }

        public void Stop()
        {
            _logger.Info($"Stopping projection processor for aggregate type '{typeof(T).Name}'...");

            cancellationToken.Cancel();
        }
    }
}
