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
        public DbSet<Consultation> ConsultationTable { get; set; }
        public DbSet<AppointmentDetails> AppointmentsTable { get; set; }
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
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationID);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.SentDate).IsRequired();
                entity.Property(e => e.IsRead).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserID)
                      .OnDelete(DeleteBehavior.NoAction); 
            });
        }
    }
}
