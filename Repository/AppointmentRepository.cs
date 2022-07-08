using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppointmentContext appointmentContext) : base(appointmentContext)
        {
        }

        public IEnumerable<Appointment> GetAllAppointments()
        {
            return GetAll().OrderBy(pr => pr.AppointmentStart).ToList();
        }
    }
}
