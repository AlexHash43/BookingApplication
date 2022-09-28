using Contracts;
using Entities;
using Entities.DataTransferObjects.DoctorScheduleDtos;
using Entities.Models;
using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DoctorScheduleRepository : RepositoryBase<DoctorSchedule>, IDoctorScheduleRepository
    {
        private readonly AppointmentContext _context;

        public DoctorScheduleRepository(AppointmentContext appointmentContext ) : base(appointmentContext)
        {
            _context = appointmentContext;
        }
        public async Task<IEnumerable<DoctorSchedule>> GetAllSchedulesAsync(TimeFrameDto timeFrame)
        {
            return await GetAll().OrderBy(ds => ds.ConsultationStart >= timeFrame.End || ds.ConsultationEnd <= timeFrame.Start).Include(e => e.DoctorAppointment.FullName).ToListAsync();
        }

        public async Task<DoctorSchedule> GetScheduleSlotById(Guid slotId)
        {
            return await GetByCondition(ds => ds.Id == slotId).FirstOrDefaultAsync();
        }
        public async Task<List<DoctorSchedule>> GetScheduleSlotsByDoctorAsync(GetScheduleByDoctorIdDto getDoctorSchedule)
        {
            return await GetByCondition(ds => ds.DoctorId == getDoctorSchedule.DoctorId && (ds.ConsultationStart >= getDoctorSchedule.End || ds.ConsultationEnd <= getDoctorSchedule.Start)).ToListAsync();
        }
        public async Task<List<Guid>> GetDoctorIdsAsync()
        {
            var doctorRoleId = await _context.Roles.Where(r => r.NormalizedName == "DOCTOR")
                    .Select(role => role.Id)
                    .FirstOrDefaultAsync();
            var doctorIds = new List<Guid>();
            if (doctorRoleId != null)
            {
                doctorIds = doctorRoleId != null ? 
                    await _context.UserRoles.Where(r => r.RoleId == doctorRoleId)
                    .Select(a => a.UserId)
                    .ToListAsync()
                    : doctorIds;
                //return doctorIds;               
            }
            return doctorIds;
        }

    }
}
