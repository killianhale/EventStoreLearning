using System;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModel.Repositories;
using EventStoreLearning.Appointment.ReadModels.Models;
using ContextRunner;
using MediatR;
using Microsoft.Extensions.Logging;
using AggregateOP.MediatR;
using AggregateOP.Base;

namespace EventStoreLearning.Appointment.Projection
{
    public class AppointmentProjectionProcessor : MediatedProjectionProcessor<Appointment>
    {
        private readonly IContextRunner _runner;
        private readonly IProjectionInfoRepository _infoRepo;
        private ProjectionReadModel _info;

        public AppointmentProjectionProcessor(
            IContextRunner runner,
            ILogger<AppointmentProjectionProcessor> logger,
            IProjectionInfoRepository infoRepo,
            IEventRepository repo,
            IMediator eventMediator
            ) :base(runner, logger, repo, eventMediator)
        {
            _runner = runner;
            _infoRepo = infoRepo;
        }

        protected async override Task<long> GetStartEventPosition()
        {
            return await _runner.RunAction(async context =>
            {
                context.Logger.Debug($"Getting start position for Aggregate {nameof(Appointment)}");

                _info = await _infoRepo.RetrieveProjectionInfo();

                context.State.SetParam("projectionInfo", _info);

                return _info.Version + 1;
            });
        }

        protected async override Task SaveEventPosition(long position)
        {
            await _runner.RunAction(async context =>
            {
                context.Logger.Debug($"Saving the start position for Aggregate {nameof(Appointment)} as {position}");

                _info.Version = position;

                await _infoRepo.UpdateProjectionInfo(_info);
            });
        }
    }
}
