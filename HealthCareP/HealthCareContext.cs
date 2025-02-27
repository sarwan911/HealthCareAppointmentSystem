using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCareAppointmentSystem;
using Microsoft.EntityFrameworkCore;

namespace HealthCareAppointmentSystem
{
    internal class HealthCareContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AppointmentDetails> AppointmentDetails { get; set; }
        public DbSet<DocAvailability> DocAvailability { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HealthCareProject;Encrypt=false;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<AppointmentDetails>().ToTable("Appointments");
            modelBuilder.Entity<DocAvailability>().ToTable("DocAvailabilities");
            modelBuilder.Entity<AppointmentDetails>()
                .HasOne<User>(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AppointmentDetails>()
                .HasOne<User>(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DocAvailability>()
                .HasOne(d => d.Doctor)
                .WithMany()
                .HasForeignKey(d => d.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
