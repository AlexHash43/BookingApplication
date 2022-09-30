using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.DoctorScheduleDtos
{
    public class ScheduleSlotDto
    {
        public Guid Id { get; set; }
        public DateTime ConsultationStart { get; set; }
        public DateTime ConsultationEnd { get; set; }
        public Guid DoctorId { get; set; }
        public DoctorScheduleStatus ScheduleStatus { get; set; }
    }
}
