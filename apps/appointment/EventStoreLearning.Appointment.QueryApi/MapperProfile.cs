using System;
using System.Collections.Generic;
using AutoMapper;
using EventStoreLearning.Appointment.Commands;
using EventStoreLearning.Appointment.ReadModels.Models;
using EventStoreLearning.Common.Web.Models;
using Contract = EventStoreLearning.Appointment.QueryApi.Contract;

namespace EventStoreLearning.Appointment.QueryApi
{
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AppointmentReadModel, Contract.Appointment>()
                //.ForMember(dest => dest.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(dto => TimeSpan.Parse(dto.Duration).TotalMinutes));

            CreateMap<Exception, ErrorResponse>();
        }
    }
}
