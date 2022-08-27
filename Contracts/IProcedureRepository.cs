using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProcedureRepository: IRepositoryBase<Procedure>
    {
        Task <IEnumerable<Procedure>> GetAllProceduresAsync();
        Task<Procedure> GetProcedureByIdAsync(Guid id);
        Task<Procedure> GetProcedureByNameAsync(string name);
        //Task<Procedure> UpdateProcedureAsync(Procedure procedure);
        //Task DeleteProcedureAsync(Procedure procedure);
        void CreateProcedure(Procedure procedure);

    }
}
