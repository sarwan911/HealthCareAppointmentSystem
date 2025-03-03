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
            Console.WriteLine("Welcome to the Healthcare Appointment Management System!");

            while (!exit)
            {
                Console.WriteLine("\nMain Menu");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Book a new appointment");
                Console.WriteLine("4. List all appointments");
                Console.WriteLine("5. Modify an appointment");
                Console.WriteLine("6. Cancel an appointment");
                Console.WriteLine("7. View doctor availability");
                Console.WriteLine("8. Add doctor availability");
                Console.WriteLine("9. Update doctor availability");
                Console.WriteLine("10. Delete doctor availability");
                Console.WriteLine("11. Reschedule an appointment");
                Console.WriteLine("12. Assign a new appointment");
                Console.WriteLine("13. Add Consultation");
                Console.WriteLine("14. List Consultations");
                Console.WriteLine("15. Exit");
                Console.Write("Select an option: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            RegisterUser(controller);
                    
                            break;
                        case 2:
                            LoginUser(controller);
                            break;
                        case 3:
                            CreateAppointment(controller);
                            break;
                        case 4:
                            ListAppointments(controller);
                            break;
                        case 5:
                            UpdateAppointment(controller);
                            break;
                        case 6:
                            DeleteAppointment(controller);
                            break;
                        case 7:
                            ListDocAvailabilities(controller);
                            break;
                        case 8:
                            CreateDocAvailability(controller);
                            break;
                        case 9:
                            UpdateDocAvailability(controller);
                            break;
                        case 10:
                            DeleteDocAvailability(controller);
                            break;
                        case 11:
                            RescheduleAppointment(controller);
                            break;
                        case 12:
                            AssignNewAppointment(controller);
                            break;
                        case 13:
                            AddConsultation(controller);
                            break;
                        case 14:
                            ListConsultations(controller);
                            break;
                        case 15:
                            Console.WriteLine("Exiting the application...");
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }
        static void RegisterUser(HealthCareController controller)
        {
            Console.WriteLine("\nRegister a New User");

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Role (Doctor/Patient): ");
            string role = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Phone: ");
            long phone = long.Parse(Console.ReadLine());

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            var newUser = new User
            {
                Name = name,
                Role = role,
                Email = email,
                Age = age,
                Phone = phone,
                Address = address,
                Password = password
            };
            insert inc = new insert();
            inc.insertUser(newUser);

            controller.RegisterUser(newUser);
            Console.WriteLine("User registered successfully!");
        }

        static void LoginUser(HealthCareController controller)
        {
            Console.WriteLine("\nLogin");

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            var user = controller.LoginUser(email, password, out _);
            if (user == 1)
            {
                Console.WriteLine($"Login successful.");
            }
            else
            {
                Console.WriteLine("Invalid email or password.");
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
        static void AddConsultation(HealthCareController controller)
        {
            Console.WriteLine("\nAdd Consultation");

            Console.Write("Enter Appointment ID: ");
            var appointmentId = int.Parse(Console.ReadLine());
            Console.Write("Enter Consultation Notes: ");
            var notes = Console.ReadLine();
            Console.Write("Enter Prescription: ");
            var prescription = Console.ReadLine();

            controller.AddConsultation(appointmentId, notes, prescription);
            Console.WriteLine("Consultation added successfully!");
        }

        static void ListConsultations(HealthCareController controller)
        {
            Console.WriteLine("\nList of Consultations:");
            var consultations = controller.ListConsultations();
            foreach (var consultation in consultations)
            {
                Console.WriteLine($"Appointment ID: {consultation.AppointmentID}, Notes: {consultation.Notes}, Prescription: {consultation.Prescription}");
            }
        }
    }
}