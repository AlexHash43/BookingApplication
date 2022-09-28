using Entities.DataTransferObjects.DoctorScheduleDtos;
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
        Task<IEnumerable<DoctorSchedule>> GetAllSchedulesAsync(TimeFrameDto interval);
        Task<DoctorSchedule> GetScheduleSlotById(Guid slotId);
        Task<List<DoctorSchedule>> GetScheduleSlotsByDoctorAsync(GetScheduleByDoctorIdDto getDoctorSchedule);
        Task<List<Guid>> GetDoctorIdsAsync();
    }
}
