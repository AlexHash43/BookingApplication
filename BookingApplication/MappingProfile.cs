using AutoMapper;
using Entities.Models;
using Entities.DataTransferObjects;

namespace BookingApplication
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Procedure, ProcedureDto>();
        }
    }
}
