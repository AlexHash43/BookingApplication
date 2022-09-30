using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.DoctorScheduleDtos
{
    public class SlotUpdateDto
    {
        public Guid Id { get; set; }
        public DoctorScheduleStatus ScheduleStatus { get; set; }
    }
}
