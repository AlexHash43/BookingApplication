
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.AppointmentDtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entities.Models.Enums;

namespace Repository
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppointmentContext appointmentContext) : base(appointmentContext)
        {
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(DateTime start, DateTime end)
        {
                return await GetByCondition(ap => !((ap.AppointmentStart <= end) || (ap.AppointmentEnd >= start)))
                .Include(e=> e.DoctorSchedule.DoctorAppointment.FullName).Include(e => e.PatientAppointment.FullName).OrderBy(ap => ap.AppointmentStart).ToListAsync();

        }
        public async Task<Appointment> GetAppointmentByIdAsync(Guid appId)
        {
            return await GetByCondition(ap => ap.Id == appId).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Appointment>> ExistingAppointment(AppointmentRange appointmentSlotRange)
        {
            var existingAppointment = await GetByCondition(ap => !((ap.AppointmentEnd >= appointmentSlotRange.Start) || (ap.AppointmentStart <= appointmentSlotRange.End))).ToListAsync();
            return existingAppointment;
        }

        public async Task<IEnumerable<Appointment>> GetDoctorAppointments(AppointmentRange appointmentRange)
        {
            return await GetByCondition(ap => !((ap.AppointmentStart <= appointmentRange.End) || (ap.AppointmentEnd >= appointmentRange.Start)) && ap.DoctorSchedule.DoctorId == appointmentRange.Id)
            .Include(e => e.DoctorSchedule.DoctorAppointment.FullName).OrderBy(ap => ap.AppointmentStart).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetPatientAppointments(AppointmentRange appointmentRange)
        {
            return await GetByCondition(ap => !((ap.AppointmentStart <= appointmentRange.End) || (ap.AppointmentEnd >= appointmentRange.Start)) && ap.PatientId == appointmentRange.Id && ap.Status != AppointmentStatus.Open).Include(e => e.DoctorSchedule.DoctorAppointment.FullName).OrderBy(ap => ap.AppointmentStart).ToListAsync();
            //(e => (e.Status != AppointmentStatus.Open && e.PatientId == patient)) && !((e.End <= start) || (e.Start >= end))).Include(e => e.Doctor).ToListAsync();
        }

        public Appointment CreateAppointmentAsync (AppointmentCreationDto appointmentToCreate)
        {
            var newAppointment = new Appointment();

                newAppointment.DoctorScheduleId = appointmentToCreate.ScheduleSlotId;
                newAppointment.PatientId = appointmentToCreate.PatientId;
                newAppointment.ProcedureId = appointmentToCreate.ProcedureId;
                newAppointment.AppointmentStart = appointmentToCreate.AppointmentStart;
                newAppointment.AppointmentEnd = appointmentToCreate.AppointmentEnd;
                newAppointment.Status = AppointmentStatus.Scheduled;
                newAppointment.Description = appointmentToCreate.Description;
                newAppointment.UpdatedOn = DateTime.Now;
            return newAppointment;
        }


    }
}
