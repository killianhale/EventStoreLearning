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
                .ConstructUsing(req => new CreateAppointmentCommand(Guid.NewGuid(), req.Title, req.StartTime, TimeSpan.FromMinutes(req.DurationMinutes)));
                //.ForMember(dest => dest.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                //.ForMember(dest => dest.Duration, opt => opt.MapFrom(dto => TimeSpan.FromMinutes(dto.DurationMinutes)));

            CreateMap<Exception, ErrorResponse>();
        }
    }
}
