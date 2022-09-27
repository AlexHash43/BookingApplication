using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDoctorScheduleRepository : IRepositoryBase<DoctorSchedule>
    {
        Task<IEnumerable<DoctorSchedule>> GetAllSchedulesAsync();
        Task<DoctorSchedule> GetSchedulesByIdAsync(Guid id);
        Task<DoctorSchedule> GetSchedulesByDoctorAsync(string name);
    }
}
