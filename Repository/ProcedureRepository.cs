using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
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

        public async  Task<IEnumerable<Procedure>> GetAllProceduresAsync()
        {
            return await GetAll().OrderBy(pr => pr.ProcedureName).ToListAsync();
        }

        public async Task<Procedure> GetProcedureByIdAsync(Guid id)
        {
            return await GetByCondition(pr => pr.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Procedure> GetProcedureByNameAsync(string name)
        {
            return await GetByCondition(pr => pr.ProcedureName.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

        //public void UpdateProcedureAsync(Procedure procedure)
        //{
        //    Update(procedure);
        //    //return  await GetByCondition(pr => pr.Id == procedure.Id).FirstOrDefaultAsync();
        //}
        //public void CreateProcedure(Procedure procedure)
        //{
        //    Create(procedure);
        //}
        //public void DeleteProcedureAsync(Procedure procedure)
        //{
        //    Delete(procedure);
        //}
    }
}
