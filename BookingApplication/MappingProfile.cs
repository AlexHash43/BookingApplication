﻿using AutoMapper;
using Entities.Models;
using Entities.DataTransferObjects;

namespace BookingApplication
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Procedure, ProcedureDto>();
            CreateMap<ProcedureCreationDto, Procedure>();
            CreateMap<Appointment, AppointmentDto>();
            CreateMap<AppointmentCreationDto, Appointment>();
            CreateMap<ProcedureForUpdateDto, Procedure>();
        }
    }
}
