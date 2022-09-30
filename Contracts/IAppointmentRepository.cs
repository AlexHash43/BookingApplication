using Entities.DataTransferObjects;
using Entities.DataTransferObjects.AppointmentDtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAppointmentRepository:IRepositoryBase<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(DateTime start, DateTime end);
        Task<Appointment> GetAppointmentByIdAsync(Guid appId);
        Task<IEnumerable<Appointment>> GetDoctorAppointments(AppointmentRange appointmentRange);
        Task<IEnumerable<Appointment>> GetPatientAppointments(AppointmentRange appointmentRange);
        Appointment CreateAppointmentAsync(AppointmentCreationDto appointmentToCreate);
        //Task<Procedure> UpdateProcedureAsync(Procedure procedure);
        //Task DeleteProcedureAsync(Procedure procedure);
        //void CreateAppointment(Appointment appointment);
    }
}
