using System;
using AutoMapper;
using EventStoreLearning.Appointment.CommandApi.Contract;
using EventStoreLearning.Appointment.Commands;
using EventStoreLearning.Common.Web.Models;

namespace EventStoreLearning.Appointment.CommandApi
{
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateAppointmentRequest, CreateAppointmentCommand>()
                .ConstructUsing(req => new CreateAppointmentCommand(req.Title, req.StartTime, TimeSpan.FromMinutes(req.DurationMinutes)));

            CreateMap<ChangeAppointmentRequest, ChangeAppointmentCommand>()
                .ConstructUsing((req, context) =>
                {
                    var duration = req.DurationMinutes.HasValue
                        ? TimeSpan.FromMinutes(req.DurationMinutes.Value)
                        : (TimeSpan?)null;

                    return new ChangeAppointmentCommand(
                        req.Id,
                        req.Version,
                        req.Title,
                        req.StartTime,
                        duration);
                });

            CreateMap<CancelAppointmentRequest, CancelAppointmentCommand>()
                .ConstructUsing(req => new CancelAppointmentCommand(req.Id, req.Version));

            CreateMap<Exception, ErrorResponse>();
        }
    }
}
