using Entities.Models;
using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Appointment start time is required")]
        public Guid DoctorScheduleId { get; set; }
        public Guid ProcedureId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime AppointmentStart { get; set; }
        public DateTime AppointmentEnd { get; set; }
        public DateTime UpdatedOn { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Description { get; set; }

    }
}
