using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.DoctorScheduleDtos
{
    public class GetScheduleByDoctorIdDto
    {
        public Guid DoctorId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DoctorScheduleStatus ScheduleStatus { get; set;}
    } 
}
