using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HealthCareAppointmentSystem
{
    internal class HealthCareController
    {
        private readonly HealthCareContext _context;

        public HealthCareController()
        {
            _context = new HealthCareContext();
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
    }
}
