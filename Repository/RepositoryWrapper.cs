using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private AppointmentContext _appointmentContext;
        private IAppointmentRepository _appointment;
        private IProcedureRepository _procedure;
        private IDoctorScheduleRepository _doctorSchedule;

        public IAppointmentRepository Appointment
        {
            get
            {
                if(_appointment==null)
                {
                    _appointment = new AppointmentRepository(_appointmentContext);
                }
                return _appointment;
            }
        }
        public IProcedureRepository Procedure
        {
            get
            {
                if (_procedure == null)
                {
                    _procedure = new ProcedureRepository(_appointmentContext);
                }
                return _procedure;
            }
        }
        public IDoctorScheduleRepository DoctorSchedule
        {
            get
            {
                if (_doctorSchedule == null)
                {
                    _doctorSchedule = new DoctorScheduleRepository(_appointmentContext);
                }
                return _doctorSchedule;
            }
        }
        
        public RepositoryWrapper(AppointmentContext appointmentContext)//, IAppointmentRepository appointment, IProcedureRepository procedure)
        {
            _appointmentContext = appointmentContext;
            //_appointment = appointment;
            //_procedure = procedure;
        }
        public async Task<int> SaveAsync()
        {
           return await _appointmentContext.SaveChangesAsync();
            
        }
    }
}
