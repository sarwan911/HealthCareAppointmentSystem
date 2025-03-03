using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Notification2.Data;
using Notification2.Models;

namespace Notification2.Services
{
    public class NotificationService
    {
        private readonly NotificationContext _context;

        public NotificationService(NotificationContext context)
        {
            _context = context;
        }

        public async Task SendAppointmentReminder(int appointmentId)
        {
            var appointment = await _context.AppointmentsTable
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentID == appointmentId);

            if (appointment != null)
            {
                var message = $"Reminder: You have an appointment with Dr. {appointment.Doctor.Name} on {appointment.Date} at {appointment.TimeSlot}.";
                var notification = new Notification
                {
                    UserID = appointment.PatientID,
                    Message = message,
                    SentDate = DateTime.Now,
                    IsRead = false
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                // Send email/SMS or display in frontend
                // EmailService.SendEmail(appointment.Patient.Email, "Appointment Reminder", message);
                // SmsService.SendSms(appointment.Patient.Phone, message);
            }
        }

        public async Task NotifyCancellation(int appointmentId)
        {
            var appointment = await _context.AppointmentsTable
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentID == appointmentId);

            if (appointment != null)
            {
                var message = $"Notification: Your appointment with Dr. {appointment.Doctor.Name} on {appointment.Date} at {appointment.TimeSlot} has been cancelled.";
                var notification = new Notification
                {
                    UserID = appointment.PatientID,
                    Message = message,
                    SentDate = DateTime.Now,
                    IsRead = false
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                // Send email/SMS or display in frontend
                // EmailService.SendEmail(appointment.Patient.Email, "Appointment Cancellation", message);
                // SmsService.SendSms(appointment.Patient.Phone, message);
            }
        }
    }
}