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
    public class ProcedureRepository : RepositoryBase<Procedure>, IProcedureRepository
    {
        public ProcedureRepository(AppointmentContext appointmentContext) : base(appointmentContext)
        {
        }

        public IEnumerable<Procedure> GetAllProcedures()
        {
            return GetAll().OrderBy(pr => pr.ProcedureName).ToList();
        }
    }
}
