using Entities.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class DoctorSchedule
    {
        public Guid Id { get; set; }
        //[Required(ErrorMessage = "Appointment start time is required")]
        public DateTime ConsultationStart { get; set; }
        public DateTime ConsultationEnd { get; set; }
        [ForeignKey(nameof(DoctorAppointment))]
        public Guid DoctorId { get; set; }
        public DoctorScheduleStatus ScheduleStatus { get; set; }
        public virtual User? DoctorAppointment { get; set; }

    }
}
