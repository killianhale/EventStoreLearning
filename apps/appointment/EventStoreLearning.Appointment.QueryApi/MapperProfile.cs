using System;
using AutoMapper;
using EventStoreLearning.Appointment.ReadModels.Models;
using EventStoreLearning.Common.Web.Models;

namespace EventStoreLearning.Appointment.QueryApi
{
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AppointmentReadModel, Contract.Appointment>()
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(dto => dto.Duration));

            CreateMap<Exception, ErrorResponse>();
        }
    }
}
