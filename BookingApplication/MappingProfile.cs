using AutoMapper;
using Entities.Models;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.UserDtos;
using Entities.DataTransferObjects.DoctorScheduleDtos;

namespace BookingApplication
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Procedure, ProcedureDto>();
            CreateMap<ProcedureCreationDto, Procedure>();
            CreateMap<ProcedureForUpdateDto, Procedure>();
            CreateMap<Appointment, AppointmentDto>();
            CreateMap<AppointmentCreationDto, Appointment>();
            CreateMap<AppointmentForUpdateDto, Appointment>();
            CreateMap<User, GetUserDto>();
            CreateMap<GetUserDto, User>();
            CreateMap<SlotUpdateDto, DoctorSchedule>();
            CreateMap<ScheduleSlotDto, DoctorSchedule>();
        }
    }
}
