using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.DoctorScheduleDtos
{
    public class ScheduleSlotRangeDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Weekends { get; set; }
    }
}
