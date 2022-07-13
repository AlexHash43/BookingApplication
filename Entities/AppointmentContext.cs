using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public  class AppointmentContext: IdentityDbContext<User>
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options) { }

        //public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Procedure> ProcedureTypes { get; set; }
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
