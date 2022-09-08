using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class DoctorSchedule
    {
        public Guid Id { get; set; }
        //[Required(ErrorMessage = "Appointment start time is required")]
        public DateTime AppointmentStart { get; set; }
        public DateTime AppointmentEnd { get; set; }

        [ForeignKey(nameof(DoctorAppointment))]
        public Guid DoctorId { get; set; }
    }
}
