using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCareAppointmentSystem;
using Microsoft.EntityFrameworkCore;
using Notification2.Models;

namespace HealthCareAppointmentSystem
{
    internal class HealthCareContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AppointmentDetails> AppointmentDetails { get; set; }
        public DbSet<DocAvailability> DocAvailability { get; set; }
        public DbSet<Consultation> ConsultationTable { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HealthCare;Encrypt=false;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserID); // Primary key
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Role).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Age).IsRequired();
                entity.Property(u => u.Phone).IsRequired();
                entity.Property(u => u.Address).IsRequired();
            });
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
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Age).IsRequired();
                entity.Property(e => e.Phone).IsRequired();
                entity.Property(e => e.Address).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<AppointmentDetails>(entity =>
            {
                entity.HasKey(e => e.AppointmentID);
                entity.Property(e => e.PatientID).IsRequired();
                entity.Property(e => e.DoctorID).IsRequired();
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.TimeSlot).IsRequired();
                entity.Property(e => e.Location).IsRequired();
                entity.HasOne(e => e.Patient)
                      .WithMany()
                      .HasForeignKey(e => e.PatientID)
                      .OnDelete(DeleteBehavior.NoAction); // Specify NO ACTION
                entity.HasOne(e => e.Doctor)
                      .WithMany()
                      .HasForeignKey(e => e.DoctorID)
                      .OnDelete(DeleteBehavior.NoAction); // Specify NO ACTION
            });            
        }
    }
}
