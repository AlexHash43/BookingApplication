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
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(DateTime start, DateTime end, Guid doctorId);
        Task<Appointment> GetAppointmentByIdAsync(Guid appId);
        Task<IEnumerable<Appointment>> GetDoctorAppointments(Guid doctorId);
        Task<IEnumerable<Appointment>> GetPatientAppointments(Guid patientId);
        //Task<Procedure> UpdateProcedureAsync(Procedure procedure);
        //Task DeleteProcedureAsync(Procedure procedure);
        //void CreateAppointment(Appointment appointment);
    }
}
