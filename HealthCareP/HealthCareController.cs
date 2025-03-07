using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCareAppointmentSystem
{
    public class HealthCareController
    {
        private readonly HealthCareContext _context;
        public HealthCareController()
        {
            _context = new HealthCareContext();
        }
        // ---------------------- User Management ----------------------
        public void RegisterUser(User user)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user);
            if (!Validator.TryValidateObject(user, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                    Console.WriteLine(validationResult.ErrorMessage);
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
        //public int LoginUser(string email, string password)
        //{
        //    var emailParam = new SqlParameter("@Email", email);
        //    var passwordParam = new SqlParameter("@Password", password);
        //    var resultParam = new SqlParameter("@res", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
        //    _context.Database.ExecuteSqlRaw("EXEC LoginUser @Email, @Password, @res OUTPUT", emailParam, passwordParam, resultParam);
        //    return (int)resultParam.Value;
        //}
        public string LoginUser(string email, string password)
        {
            var emailParam = new SqlParameter("@Email", email);
            var passwordParam = new SqlParameter("@Password", password);
            var roleParam = new SqlParameter("@Role", System.Data.SqlDbType.VarChar, 20)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            _context.Database.ExecuteSqlRaw("EXEC LoginUser @Email, @Password, @Role OUTPUT", emailParam, passwordParam, roleParam);
            return roleParam.Value?.ToString(); // Returns "doctor", "patient", or null if login fails
        }
        //public int LoginUser(string email, string password, out bool isPasswordReset, string answer = null, string newPassword = null)
        //{
        //    isPasswordReset = false;
        //    if (!string.IsNullOrEmpty(password)) // Normal Login
        //    {
        //        var emailParam = new SqlParameter("@Email", email);
        //        var passwordParam = new SqlParameter("@Password", password);
        //        var resultParam = new SqlParameter("@res", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
        //        _context.Database.ExecuteSqlRaw("EXEC LoginUser @Email, @Password, @res OUTPUT", emailParam, passwordParam, resultParam);
        //        return (int)resultParam.Value;
        //    }
        //    else if (!string.IsNullOrEmpty(answer) && !string.IsNullOrEmpty(newPassword)) // Forgot Password Flow
        //    {
        //        var emailParam = new SqlParameter("@Email", email);
        //        var answerParam = new SqlParameter("@Answer", answer);
        //        var newPasswordParam = new SqlParameter("@NewPassword", newPassword);
        //        int rowsAffected = _context.Database.ExecuteSqlRaw("EXEC ForgotPassword @Email, @Answer, @NewPassword", emailParam, answerParam, newPasswordParam);
        //        if (rowsAffected > 0)
        //        {
        //            isPasswordReset = true;
        //            return 1;
        //        }
        //        return 0;
        //    }
        //    return 0;
        //}
        // ---------------------- Appointment Management ----------------------
        public List<AppointmentDetails> ListAppointments()
        {
            return _context.AppointmentDetails.ToList();
        }
        public void CreateAppointment(AppointmentDetails newAppointment)
        {
            _context.AppointmentDetails.Add(newAppointment);
            _context.SaveChanges();
            SendNotification(newAppointment.PatientID, "Your appointment has been created successfully.");
        }
        public void UpdateAppointment(AppointmentDetails updated)
        {
            var existing = _context.AppointmentDetails.Find(updated.AppointmentID);
            if (existing == null) return;
            existing.PatientID = updated.PatientID;
            existing.DoctorID = updated.DoctorID;
            existing.Date = updated.Date;
            existing.TimeSlot = updated.TimeSlot;
            existing.Location = updated.Location;
            _context.SaveChanges();
        }
        public void DeleteAppointment(int appointmentId)
        {
            var existing = _context.AppointmentDetails.Find(appointmentId);
            if (existing == null) return;
            _context.AppointmentDetails.Remove(existing);
            _context.SaveChanges();
            SendNotification(existing.PatientID, "Your appointment has been cancelled. Please book a new appointment.");
        }
        public AppointmentDetails GetAppointmentById(int appointmentId)
        {
            return _context.AppointmentDetails.Find(appointmentId);
        }
        // ---------------------- Doctor Availability ----------------------
        public List<DocAvailability> ReadAllDocAvailabilities()
        {
            return _context.DocAvailability.ToList();
        }
        public void CreateDocAvailability(DocAvailability newAvailability)
        {
            _context.DocAvailability.Add(newAvailability);
            _context.SaveChanges();
        }
        public void UpdateDocAvailability(DocAvailability updated)
        {
            var existing = _context.DocAvailability.Find(updated.SessionID);
            if (existing == null) return;
            existing.DoctorID = updated.DoctorID;
            existing.Date = updated.Date;
            existing.TimeSlot = updated.TimeSlot;
            _context.SaveChanges();
        }
        public void DeleteDocAvailability(int sessionId)
        {
            var existing = _context.DocAvailability.Find(sessionId);
            if (existing == null) return;
            _context.DocAvailability.Remove(existing);
            _context.SaveChanges();
        }
        public DocAvailability GetDocAvailabilityById(int sessionId)
        {
            return _context.DocAvailability.Find(sessionId);
        }
        // ---------------------- Appointment Rescheduling ----------------------
        public void RescheduleAppointment(int appointmentId, DateOnly newDate, TimeOnly newTimeSlot)
        {
            _context.Database.ExecuteSqlRaw("EXEC RescheduleAppointment @p0, @p1, @p2", appointmentId, newDate, newTimeSlot);
        }
        public void AssignNewAppointment(int patientId, int doctorId, DateOnly newDate, TimeOnly newTimeSlot, string location)
        {
            _context.Database.ExecuteSqlRaw("EXEC AssignNewAppointment @p0, @p1, @p2, @p3, @p4", patientId, doctorId, newDate, newTimeSlot, location);
        }
        // ---------------------- Consultations ----------------------
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
            var appointment = _context.AppointmentDetails.Find(appointmentId);
            if (appointment != null)
                SendNotification(appointment.PatientID, "Your consultation notes and prescription are ready.");
        }
        public List<Consultation> ListConsultations()
        {
            return _context.ConsultationTable.ToList();
        }
        // ---------------------- Notifications ----------------------
        public void SendNotification(int userId, string message)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@Message", message)
            };
            _context.Database.ExecuteSqlRaw("EXEC InsertNotification @UserID, @Message", parameters);
        }
    }
}