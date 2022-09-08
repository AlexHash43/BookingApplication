
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.AppointmentDtos;
using Entities.Models;
using Repository.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Repository
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppointmentContext appointmentContext) : base(appointmentContext)
        {
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(DateTime start, DateTime end, Guid doctorId)
        {
            if (doctorId == null)
            {
                return await GetByCondition(ap => !((ap.AppointmentStart >= end) || (ap.AppointmentEnd <= start)))
                .Include(e=> e.DoctorAppointment.FullName).OrderBy(ap => ap.AppointmentStart).ToListAsync();
            }
            else
            {
                return await GetByCondition(ap => ap.DoctorId == doctorId && !((ap.AppointmentStart >= end) || (ap.AppointmentEnd <= start)))
                .Include(e => e.DoctorAppointment.FullName).OrderBy(ap => ap.AppointmentStart).ToListAsync();
            }
        }
        public async Task<IEnumerable<Appointment>> ExistingAppointment(AppointmentSlotRange appointmentSlotRange)
        {
            var existingAppointment = await GetByCondition(ap => !((ap.AppointmentEnd <= appointmentSlotRange.Start) || (ap.AppointmentStart >= appointmentSlotRange.End))).ToListAsync();
            return existingAppointment;
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid appId)
        {
            return await GetByCondition(ap => ap.Id == appId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Appointment>> GetDoctorAppointments(Guid doctorId)
        {
            return await GetByCondition(ap => ap.DoctorId== doctorId).OrderBy(ap => ap.AppointmentStart).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetPatientAppointments(Guid patientId)
        {
            return await GetByCondition(ap => ap.PatientId == patientId).OrderBy(ap => ap.AppointmentStart).ToListAsync();
            //(e => (e.Status != AppointmentStatus.Open && e.PatientId == patient)) && !((e.End <= start) || (e.Start >= end))).Include(e => e.Doctor).ToListAsync();
        }


    }
}
