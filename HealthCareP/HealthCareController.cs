using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HealthCareAppointmentSystem
{
    public class HealthCareController
    {
        private readonly HealthCareContext _context;

        public HealthCareController()
        {
            _context = new HealthCareContext();
        }
        public void RegisterUser(User user)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user);

            if (!Validator.TryValidateObject(user, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    Console.WriteLine(validationResult.ErrorMessage);
                }
                return;
            }

            var parameters = new[]
            {
                new SqlParameter("@Name", user.Name),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Age", user.Age),
                new SqlParameter("@Phone", user.Phone),
                new SqlParameter("@Address", user.Address),
                new SqlParameter("@Password", user.Password)
            };

            _context.Database.ExecuteSqlRaw("EXEC AddUser @Name, @Role, @Email, @Age, @Phone, @Address, @Password", parameters);
        }
        // Login a user
        public int LoginUser(string Email, string Password, out bool isPasswordReset, string answer = null, string newPassword = null)
        {
            isPasswordReset = false;
            if (Password != null)
            {
                var u = new SqlParameter("@Email", Email);
                var p = new SqlParameter("@Password", Password);
                var resParam = new SqlParameter("@res", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw("EXEC LoginUser @Email, @Password, @res OUTPUT", u, p, resParam);

                return (int)resParam.Value;
            }
            else if (answer != null && newPassword != null)
            {
                var paak = new SqlParameter("@Email", Email);
                var kaak = new SqlParameter("@pass", newPassword);
                var aak = new SqlParameter("@ans", answer);
                _context.Database.ExecuteSqlRaw("EXEC ChangePas @USERID, @pass, @ans", paak, kaak, aak);
                isPasswordReset = true;
                return 1;
            }
            return 0;
        }
        /// Read all appointments from the database. 
        public List<AppointmentDetails> ReadAllAppointments()
        {
            return _context.AppointmentDetails.ToList();
        }

        /// Create (book) a new appointment.
        public void CreateAppointment(AppointmentDetails newAppointment)
        {
            _context.AppointmentDetails.Add(newAppointment);
            _context.SaveChanges();
        }

        /// Update (modify) an existing appointment.
        public void UpdateAppointment(AppointmentDetails updated)
        {
            var existing = _context.AppointmentDetails.Find(updated.AppointmentID);
            if (existing != null)
            {
                existing.PatientID = updated.PatientID;
                existing.DoctorID = updated.DoctorID;
                existing.Date = updated.Date;
                existing.TimeSlot = updated.TimeSlot;
                existing.Location = updated.Location;
                _context.SaveChanges();
            }
        }

        /// Delete (cancel) an appointment by ID.
        public void DeleteAppointment(int appointmentId)
        {
            var existing = _context.AppointmentDetails.Find(appointmentId);
            if (existing != null)
            {
                _context.AppointmentDetails.Remove(existing);
                _context.SaveChanges();
            }
        }

        /// Find a single appointment by ID.
        public AppointmentDetails GetAppointmentById(int appointmentId)
        {
            return _context.AppointmentDetails.Find(appointmentId);
        }

        /// Read all doctor availabilities from the database.
        public List<DocAvailability> ReadAllDocAvailabilities()
        {
            return _context.DocAvailability.ToList();
        }

        /// Create (add) a new doctor availability.
        public void CreateDocAvailability(DocAvailability newAvailability)
        {
            _context.DocAvailability.Add(newAvailability);
            _context.SaveChanges();
        }

        /// Update an existing doctor availability.
        public void UpdateDocAvailability(DocAvailability updated)
        {
            var existing = _context.DocAvailability.Find(updated.SessionID);
            if (existing != null)
            {
                existing.DoctorID = updated.DoctorID;
                existing.Date = updated.Date;
                existing.TimeSlot = updated.TimeSlot;
                _context.SaveChanges();
            }
        }

        /// Delete a doctor availability by ID.
        public void DeleteDocAvailability(int sessionId)
        {
            var existing = _context.DocAvailability.Find(sessionId);
            if (existing != null)
            {
                _context.DocAvailability.Remove(existing);
                _context.SaveChanges();
            }
        }

        /// Find a single doctor availability by ID.
        public DocAvailability GetDocAvailabilityById(int sessionId)
        {
            return _context.DocAvailability.Find(sessionId);
        }

        /// Reschedule an appointment using a stored procedure.
        public void RescheduleAppointment(int appointmentId, DateTime newDate, TimeSpan newTimeSlot)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC RescheduleAppointment @p0, @p1, @p2",
                appointmentId, newDate, newTimeSlot);
        }

        /// Assign a new appointment using a stored procedure.
        public void AssignNewAppointment(int patientId, int doctorId, DateTime newDate, TimeSpan newTimeSlot, string location)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC AssignNewAppointment @p0, @p1, @p2, @p3, @p4",
                patientId, doctorId, newDate, newTimeSlot, location);
        }
        public void AddConsultation(int appointmentId, string notes, string prescription)
        {
            var consultation = new Consultation
            {
                AppointmentID = appointmentId,
                Notes = notes,
                Prescription = prescription
            };
            _context.ConsultationTable.Add(consultation);
            _context.SaveChanges();
        }
        public List<Consultation> ListConsultations()
        {
            var consultationDetails = new List<Consultation>();

            foreach (var consultation in _context.ConsultationTable)
            {
                consultationDetails.Add(new Consultation
                {
                    ConsultationID = consultation.ConsultationID,
                    AppointmentID = consultation.AppointmentID,
                    Notes = consultation.Notes,
                    Prescription = consultation.Prescription
                });
            }
            return consultationDetails;
        }

        /*public void ListConsultations()
        {
            foreach (var consultation in _context.ConsultationTable)
            {
                Console.WriteLine($"Consultation ID: {consultation.ConsultationID}, Appointment ID: {consultation.AppointmentID}, Notes: {consultation.Notes}, Prescription: {consultation.Prescription}");
            }
        }*/
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