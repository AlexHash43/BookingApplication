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
        public DoctorScheduleRepository(AppointmentContext appointmentContext) : base(appointmentContext)
        {
        }
        public async Task<IEnumerable<DoctorSchedule>> GetAllSchedulesAsync(TimeFrameDto timeFrame)
        {
            return await GetAll().OrderBy(ds => ds.ConsultationStart >= timeFrame.End || ds.ConsultationEnd <= timeFrame.Start).Include(e => e.DoctorAppointment.FullName).ToListAsync();
        }

        public async Task<DoctorSchedule> GetScheduleByIdAsync(Guid id)
        {
            return await GetByCondition(ds => ds.Id == id).FirstOrDefaultAsync();
        }
        public async Task<DoctorSchedule> GetScheduleByDoctorIdAsync(GetScheduleByDoctorIdDto getDoctorSchedule)
        {
            return await GetByCondition(ds => ds.Id == getDoctorSchedule.DoctorId && (ds.ConsultationStart >= getDoctorSchedule.End || ds.ConsultationEnd <= getDoctorSchedule.Start)).FirstOrDefaultAsync();
        }
        public async Task<DoctorSchedule> GetScheduleByDoctorAsync(GetScheduleByDoctorIdDto getDoctorSchedule)
        {
            return await GetByCondition(ds => ds.DoctorAppointment.FullName.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

    }
}
