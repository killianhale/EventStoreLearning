using System;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModel.Repositories;
using EventStoreLearning.Appointment.ReadModels.Models;
using EventStoreLearning.Common.EventSourcing;
using EventStoreLearning.EventStore;

namespace EventStoreLearning.Appointment.Projection
{
    public class AppointmentProjectionProcessor : ProjectionProcessor<Appointment>
    {
        private readonly IProjectionInfoRepository _infoRepo;
        private ProjectionReadModel _info;

        public AppointmentProjectionProcessor(IProjectionInfoRepository infoRepo, IAggregateStore repo, IEventMediator eventMediator)
            :base(repo, eventMediator)
        {
            _infoRepo = infoRepo;
        }

        protected async override Task<long> GetStartEventPosition()
        {
            _info = await _infoRepo.RetrieveProjectionInfo();

            return _info.Version + 1;
        }

        protected async override Task SaveEventPosition(long position)
        {
            _info.Version = position;

            await _infoRepo.UpdateProjectionInfo(_info);
        }
    }
}
