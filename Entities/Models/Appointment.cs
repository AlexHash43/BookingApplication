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
        public Guid? Id { get; set; }
        public DateTime AppointmentStart { get; set; }
        //public DateTime End { get; set; }
        [ForeignKey(nameof(Procedure))]
        public int ProcedureId { get; set; }
        public Procedure? Procedure { get; set; }
        //public ProcedureType PrType { get; set; }
        [ForeignKey(nameof(User))]
        public string? DoctorId { get; set; }
        //public User Doctor { get; set; }
        [ForeignKey(nameof(User))]
        public string? PatientId { get; set; }
        public User? User { get; set; }
        //public User Patient { get; set; }
        public DateTime CreatedOn { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
