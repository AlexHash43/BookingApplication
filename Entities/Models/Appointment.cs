using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Appointment
    {
        [Key]
        [Column("AppointmentId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime AppointmentStart { get; set; }
        //public DateTime End { get; set; }
        [ForeignKey(nameof(AppointmentProcedure))]
        public Guid ProcedureId { get; set; }

        [ForeignKey(nameof(DoctorAppointment))]
        public Guid DoctorId { get; set; }
        
        [ForeignKey(nameof(PatientAppointment))]
        public Guid PatientId { get; set; }
        public DateTime CreatedOn { get; set; }
        public AppointmentStatus Status { get; set; }
        public virtual Procedure? AppointmentProcedure { get; set; }
        public virtual User? DoctorAppointment { get; set; }
        public virtual User? PatientAppointment { get; set; }

    }
}
