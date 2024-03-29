﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User : IdentityUser<Guid>
    {

        [StringLength(50), Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [StringLength(50), Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [StringLength(100), Display(Name = "Full Name")]
        public string? FullName { get; set; }
        [StringLength(100), Display(Name = "Address")]
        public string? Address { get; set; }
        public DateTime CreatedOn { get; set; }
        [InverseProperty(nameof(Appointment.DoctorAppointment))]
        public virtual ICollection<Appointment>? DoctorAppointments { get; set; }
        [InverseProperty(nameof(Appointment.PatientAppointment))]
        public virtual ICollection<Appointment>? PatientAppointments { get; set; }
    }
}
