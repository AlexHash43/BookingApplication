using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public  class AppointmentContext: IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Procedure> Procedure { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(e => e.DoctorAppointment)
                .WithMany(m => m.DoctorAppointments)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            //.OnDelete(DeleteBehavior.ClientCascade); 

            modelBuilder.Entity<Appointment>()
                .HasOne(e => e.PatientAppointment)
                .WithMany(m => m.PatientAppointments)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            //.OnDelete(DeleteBehavior.ClientCascade);
            // modelBuilder.Entity<IdentityUserLogin<Guid>>()
            //  .HasNoKey();
            base.OnModelCreating(modelBuilder);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Appointment>()
        //                .HasRequired(m => m.Doctor)
        //                .WithMany(t => t.DoctorAppointments)
        //                .HasForeignKey(m => m.DoctorId)
        //                .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<Appointment>()
        //                .HasRequired(m => m.Patient)
        //                .WithMany(t => t.PatientAppointments)
        //                .HasForeignKey(m => m.PatientId)
        //                .WillCascadeOnDelete(false);
        //}
        //on tableCreation makes table names singular(whithout s)
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        //    {
        //        modelBuilder.Entity<User>().Property(b => b.Email).IsRequired();
        //        //modelBuilder.Entity<User>().Property(b => b.NormalizedEmail).IsRequired();
        //        modelBuilder.Entity<User>().Property(b => b.UserName).IsRequired();
        //        //modelBuilder.Entity<User>().Property(b => b.NormalizedUserName).IsRequired();
        //        //modelBuilder.Entity<User>().Property(b => b.PasswordHash).IsRequired();
        //        //modelBuilder.Entity<User>().Property(b => b.CreatedOn).IsRequired();
        //        //modelBuilder.Entity<User>().Property(b => b.EmailConfirmed).IsRequired(false);
        //        //modelBuilder.Entity<User>().Property(b => b.PhoneNumberConfirmed).IsRequired(false);
        //        //modelBuilder.Entity<User>().Property(b => b.TwoFactorEnabled).IsRequired(false);
        //        //modelBuilder.Entity<User>().Property(b => b.LockoutEnabled).IsRequired(false);
        //        //modelBuilder.Entity<User>().Property(b => b.AccessFailedCount).IsRequired(false);

        //    }
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
