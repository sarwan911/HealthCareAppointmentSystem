using System;
using System.Collections.Generic;
using HealthCareAppointmentSystem;

namespace HealthCareAppointmentSystem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HealthCareController controller = new HealthCareController();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Book a new appointment");
                Console.WriteLine("2. List all appointments");
                Console.WriteLine("3. Modify an appointment");
                Console.WriteLine("4. Cancel an appointment");
                Console.WriteLine("5. View doctor availability");
                Console.WriteLine("6. Add doctor availability");
                Console.WriteLine("7. Update doctor availability");
                Console.WriteLine("8. Delete doctor availability");
                Console.WriteLine("9. Reschedule an appointment");
                Console.WriteLine("10. Assign a new appointment");
                Console.WriteLine("11. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine().Trim();

                switch (choice)
                {
                    case "1":
                        CreateAppointment(controller);
                        break;
                    case "2":
                        ListAppointments(controller);
                        break;
                    case "3":
                        UpdateAppointment(controller);
                        break;
                    case "4":
                        DeleteAppointment(controller);
                        break;
                    case "5":
                        ListDocAvailabilities(controller);
                        break;
                    case "6":
                        CreateDocAvailability(controller);
                        break;
                    case "7":
                        UpdateDocAvailability(controller);
                        break;
                    case "8":
                        DeleteDocAvailability(controller);
                        break;
                    case "9":
                        RescheduleAppointment(controller);
                        break;
                    case "10":
                        AssignNewAppointment(controller);
                        break;
                    case "11":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void CreateAppointment(HealthCareController controller)
        {
            Console.Write("Enter Patient ID: ");
            int patientID = int.Parse(Console.ReadLine());
            Console.Write("Enter Doctor ID: ");
            int doctorID = int.Parse(Console.ReadLine());
            Console.Write("Enter appointment date (yyyy-mm-dd): ");
            DateOnly date = DateOnly.Parse(Console.ReadLine());
            Console.Write("Enter appointment time (HH:mm): ");
            TimeOnly timeSlot = TimeOnly.Parse(Console.ReadLine());
            Console.Write("Enter location: ");
            string location = Console.ReadLine();

            var newAppointment = new AppointmentDetails
            {
                PatientID = patientID,
                DoctorID = doctorID,
                Date = date,
                TimeSlot = timeSlot,
                Location = location
            };

            controller.CreateAppointment(newAppointment);
            Console.WriteLine("Appointment booked successfully!");
        }

        private static void ListAppointments(HealthCareController controller)
        {
            Console.WriteLine("\nCurrent appointments in the database:");
            List<AppointmentDetails> allAppointments = controller.ReadAllAppointments();
            foreach (var appointment in allAppointments)
            {
                Console.WriteLine(
                    $"ID: {appointment.AppointmentID}, " +
                    $"Patient: {appointment.PatientID}, " +
                    $"Doctor: {appointment.DoctorID}, " +
                    $"Date: {appointment.Date:yyyy-MM-dd}, " +
                    $"Time: {appointment.TimeSlot}, " +
                    $"Location: {appointment.Location}"
                );
            }
        }

        private static void UpdateAppointment(HealthCareController controller)
        {
            Console.Write("Enter Appointment ID to modify: ");
            int appointmentID = int.Parse(Console.ReadLine());
            var appointment = controller.GetAppointmentById(appointmentID);
            if (appointment != null)
            {
                Console.Write("Enter new Patient ID: ");
                appointment.PatientID = int.Parse(Console.ReadLine());
                Console.Write("Enter new Doctor ID: ");
                appointment.DoctorID = int.Parse(Console.ReadLine());
                Console.Write("Enter new appointment date (yyyy-mm-dd): ");
                appointment.Date = DateOnly.Parse(Console.ReadLine());
                Console.Write("Enter new appointment time (HH:mm): ");
                appointment.TimeSlot = TimeOnly.Parse(Console.ReadLine());
                Console.Write("Enter new location: ");
                appointment.Location = Console.ReadLine();

                controller.UpdateAppointment(appointment);
                Console.WriteLine("Appointment modified successfully!");
            }
            else
            {
                Console.WriteLine("Appointment not found.");
            }
        }

        private static void DeleteAppointment(HealthCareController controller)
        {
            Console.Write("Enter Appointment ID to cancel: ");
            int appointmentID = int.Parse(Console.ReadLine());
            controller.DeleteAppointment(appointmentID);
            Console.WriteLine("Appointment canceled successfully!");
        }

        private static void ListDocAvailabilities(HealthCareController controller)
        {
            Console.WriteLine("\nCurrent doctor availabilities in the database:");
            List<DocAvailability> allAvailabilities = controller.ReadAllDocAvailabilities();
            foreach (var availability in allAvailabilities)
            {
                Console.WriteLine(
                    $"Session ID: {availability.SessionID}, " +
                    $"Doctor: {availability.DoctorID}, " +
                    $"Date: {availability.Date:yyyy-MM-dd}, " +
                    $"Time: {availability.TimeSlot}"
                );
            }
        }

        private static void CreateDocAvailability(HealthCareController controller)
        {
            Console.Write("Enter Doctor ID: ");
            int doctorID = int.Parse(Console.ReadLine());
            Console.Write("Enter availability date (yyyy-mm-dd): ");
            DateOnly date = DateOnly.Parse(Console.ReadLine());
            Console.Write("Enter availability time (HH:mm): ");
            TimeOnly timeSlot = TimeOnly.Parse(Console.ReadLine());

            var newAvailability = new DocAvailability
            {
                DoctorID = doctorID,
                Date = date,
                TimeSlot = timeSlot
            };

            controller.CreateDocAvailability(newAvailability);
            Console.WriteLine("Doctor availability added successfully!");
        }

        private static void UpdateDocAvailability(HealthCareController controller)
        {
            Console.Write("Enter Session ID to update: ");
            int sessionID = int.Parse(Console.ReadLine());
            var availability = controller.GetDocAvailabilityById(sessionID);
            if (availability != null)
            {
                Console.Write("Enter new Doctor ID: ");
                availability.DoctorID = int.Parse(Console.ReadLine());
                Console.Write("Enter new date (yyyy-mm-dd): ");
                availability.Date = DateOnly.Parse(Console.ReadLine());
                Console.Write("Enter new time slot (HH:mm): ");
                availability.TimeSlot = TimeOnly.Parse(Console.ReadLine());

                controller.UpdateDocAvailability(availability);
                Console.WriteLine("Doctor availability updated successfully!");
            }
            else
            {
                Console.WriteLine("Doctor availability not found.");
            }
        }

        private static void DeleteDocAvailability(HealthCareController controller)
        {
            Console.Write("Enter Session ID to delete: ");
            int sessionID = int.Parse(Console.ReadLine());
            controller.DeleteDocAvailability(sessionID);
            Console.WriteLine("Doctor availability deleted successfully!");
        }

        private static void RescheduleAppointment(HealthCareController controller)
        {
            Console.Write("Enter Appointment ID to reschedule: ");
            int appointmentID = int.Parse(Console.ReadLine());
            Console.Write("Enter new appointment date (yyyy-mm-dd): ");
            DateTime newDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter new appointment time (HH:mm): ");
            TimeSpan newTimeSlot = TimeSpan.Parse(Console.ReadLine());

            controller.RescheduleAppointment(appointmentID, newDate, newTimeSlot);
            Console.WriteLine("Appointment rescheduled successfully!");
        }

        private static void AssignNewAppointment(HealthCareController controller)
        {
            Console.Write("Enter Patient ID: ");
            int patientID = int.Parse(Console.ReadLine());
            Console.Write("Enter Doctor ID: ");
            int doctorID = int.Parse(Console.ReadLine());
            Console.Write("Enter new appointment date (yyyy-mm-dd): ");
            DateTime newDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter new appointment time (HH:mm): ");
            TimeSpan newTimeSlot = TimeSpan.Parse(Console.ReadLine());
            Console.Write("Enter location: ");
            string location = Console.ReadLine();

            controller.AssignNewAppointment(patientID, doctorID, newDate, newTimeSlot, location);
            Console.WriteLine("New appointment assigned successfully!");
        }
    }
}