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

            CreateMap<Exception, ErrorResponse>();
        }
    }
}
