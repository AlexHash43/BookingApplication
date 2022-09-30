using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class AppointmentCreationDto
    {
        public Guid ScheduleSlotId { get; set; }
        public Guid PatientId { get; set; }
        public Guid ProcedureId { get; set; }
        public DateTime AppointmentStart { get; set; }
        public DateTime AppointmentEnd { get; set; }
        public string? Description { get; set; }
    }
}
