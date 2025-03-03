using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Notification2.Models;

namespace Notification2.Data
{
    public class NotificationContext : DbContext
    {
        public NotificationContext(DbContextOptions<NotificationContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Notif_Databs;Encrypt=False");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AppointmentDetails> AppointmentsTable { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationID);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.SentDate).IsRequired();
                entity.Property(e => e.IsRead).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserID)
                      .OnDelete(DeleteBehavior.NoAction); // Specify NO ACTION
            });
        }
    }
}

