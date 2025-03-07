using System;
using System.Collections.Generic;
using System.Data;
using HealthCareAppointmentSystem;

namespace HealthCareAppointmentSystem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HealthCareController controller = new HealthCareController();
            bool exit = false;
            string role = string.Empty;
            Console.WriteLine("Welcome to the Healthcare Appointment Management System!");
            while (!exit)
            {
                if (string.IsNullOrEmpty(role))
                {
                    Console.WriteLine("\nMain Menu:");
                    Console.WriteLine("1. Register");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("3. Exit");
                    Console.Write("Select an option: ");
                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                RegisterUser(controller);
                                break;
                            case 2:
                                role = LoginUser(controller);
                                if (role == "patient")
                                {
                                    PatientMenu(controller, ref role);
                                }
                                else if (role == "doctor")
                                {
                                    DoctorMenu(controller, ref role);
                                }
                                break;
                            case 3:
                                Console.WriteLine("Exiting the application...");
                                exit = true;
                                break;
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
        }
        static void RegisterUser(HealthCareController controller)
        {
            Console.WriteLine("\nRegister a New User");
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Role (Doctor/Patient): ");
            string role = Console.ReadLine().ToLower();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Age: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Enter Phone: ");
            long phone = long.Parse(Console.ReadLine());
            Console.Write("Enter Address: ");
            string address = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            User newUser = new User
            {
                Name = name,
                Role = role,
                Email = email,
                Age = age,
                Phone = phone,
                Address = address,
                Password = password
            };
            controller.RegisterUser(newUser);
            Console.WriteLine("User registered successfully.");
        }
        static string LoginUser(HealthCareController controller)
        {
            Console.WriteLine("\nUser Login");
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            string role = controller.LoginUser(email, password); // Get role from database
            if (!string.IsNullOrEmpty(role))
            {
                Console.WriteLine("Login successful. Welcome, " + role + "!");
                return role.ToLower();
            }
            else
            {
                Console.WriteLine("Invalid email or password.");
                return string.Empty;
            }
        }
        //static string LoginUser(HealthCareController controller)
        //{
        //    Console.WriteLine("\nUser Login");
        //    Console.Write("Enter Email: ");
        //    string email = Console.ReadLine();
        //    Console.Write("Enter Password: ");
        //    string password = Console.ReadLine();
        //    int result = controller.LoginUser(email, password);
        //    if (result == 1)
        //    {
        //        Console.WriteLine("Login successful.");
        //        Console.Write("Enter role (Doctor/Patient): ");
        //        return Console.ReadLine().ToLower();
        //    }
        //    else
        //    {
        //        Console.WriteLine("Invalid email or password.");
        //        return string.Empty;
        //    }
        //}
        static void PatientMenu(HealthCareController controller, ref string role)
        {
            Console.WriteLine("\nPatient Menu:");
            Console.WriteLine("1. Book a new appointment");
            Console.WriteLine("2. List all appointments");
            Console.WriteLine("3. Modify an appointment");
            Console.WriteLine("4. Cancel an appointment");
            Console.WriteLine("5. View doctor availability");
            Console.WriteLine("6. Reschedule an appointment");
            Console.WriteLine("7. View Notifications");
            Console.WriteLine("8. Logout");
            Console.Write("Select an option: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        CreateAppointment(controller);
                        break;
                    case 2:
                        ListAppointments(controller);
                        break;
                    case 3:
                        UpdateAppointment(controller);
                        break;
                    case 4:
                        CancelAppointment(controller);
                        break;
                    case 5:
                        ListDocAvailabilities(controller);
                        break;
                    case 6:
                        RescheduleAppointment(controller);
                        break;
                    case 7:
                        ViewNotifications(controller);
                        break;
                    case 8:
                        Console.WriteLine("Logging out...");
                        role = string.Empty;
                        break;
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
        static void DoctorMenu(HealthCareController controller, ref string role)
        {
            Console.WriteLine("\nDoctor Menu:");
            Console.WriteLine("1. Add doctor availability");
            Console.WriteLine("2. Update doctor availability");
            Console.WriteLine("3. Delete doctor availability");
            Console.WriteLine("4. Assign a new appointment");
            Console.WriteLine("5. Add Consultation");
            Console.WriteLine("6. List Consultations");
            Console.WriteLine("7. Logout");
            Console.Write("Select an option: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        CreateDocAvailability(controller);
                        break;
                    case 2:
                        UpdateDocAvailability(controller);
                        break;
                    case 3:
                        DeleteDocAvailability(controller);
                        break;
                    case 4:
                        AssignNewAppointment(controller);
                        break;
                    case 5:
                        AddConsultation(controller);
                        break;
                    case 6:
                        ListConsultations(controller);
                        break;
                    case 7:
                        Console.WriteLine("Logging out...");
                        role = string.Empty;
                        break;
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
        static void CreateAppointment(HealthCareController controller) {
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
        static void ListAppointments(HealthCareController controller) {
            Console.WriteLine("\nCurrent appointments in the database:");
            List<AppointmentDetails> allAppointments = controller.ListAppointments();
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
        static void UpdateAppointment(HealthCareController controller) {
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
        static void CancelAppointment(HealthCareController controller) {
            Console.Write("Enter Appointment ID to cancel: ");
            int appointmentID = int.Parse(Console.ReadLine());
            controller.DeleteAppointment(appointmentID);
            Console.WriteLine("Appointment canceled successfully!");
        }
        static void ViewNotifications(HealthCareController controller) { /* Implementation */ }
        static void ListDocAvailabilities(HealthCareController controller) {
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
        static void RescheduleAppointment(HealthCareController controller)
        {
            Console.Write("Enter Appointment ID to reschedule: ");
            int appointmentID = int.Parse(Console.ReadLine());
            Console.Write("Enter new appointment date (yyyy-mm-dd): ");
            DateOnly newDate = DateOnly.Parse(Console.ReadLine());
            Console.Write("Enter new appointment time (HH:mm): ");
            TimeOnly newTimeSlot = TimeOnly.Parse(Console.ReadLine());

            controller.RescheduleAppointment(appointmentID, newDate, newTimeSlot);
            Console.WriteLine("Appointment rescheduled successfully!");
        }
        static void CreateDocAvailability(HealthCareController controller) {
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
        static void UpdateDocAvailability(HealthCareController controller) {
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
        static void DeleteDocAvailability(HealthCareController controller) {
            Console.Write("Enter Session ID to delete: ");
            int sessionID = int.Parse(Console.ReadLine());
            controller.DeleteDocAvailability(sessionID);
            Console.WriteLine("Doctor availability deleted successfully!");
        }
        static void AssignNewAppointment(HealthCareController controller) {
            Console.Write("Enter Patient ID: ");
            int patientID = int.Parse(Console.ReadLine());
            Console.Write("Enter Doctor ID: ");
            int doctorID = int.Parse(Console.ReadLine());
            Console.Write("Enter new appointment date (yyyy-mm-dd): ");
            DateOnly newDate = DateOnly.Parse(Console.ReadLine());
            Console.Write("Enter new appointment time (HH:mm): ");
            TimeOnly newTimeSlot = TimeOnly.Parse(Console.ReadLine());
            Console.Write("Enter location: ");
            string location = Console.ReadLine();

            controller.AssignNewAppointment(patientID, doctorID, newDate, newTimeSlot, location);
            Console.WriteLine("New appointment assigned successfully!");
        }
        static void AddConsultation(HealthCareController controller) {
            Console.WriteLine("\nConsultation");

            Console.Write("Enter Appointment ID: ");
            var appointmentId = int.Parse(Console.ReadLine());
            Console.Write("Enter Consultation Notes: ");
            var notes = Console.ReadLine();
            Console.Write("Enter Prescription: ");
            var prescription = Console.ReadLine();

            controller.AddConsultation(appointmentId, notes, prescription);
            Console.WriteLine("Consultation added successfully!");
        }
        static void ListConsultations(HealthCareController controller) {
            Console.WriteLine("\nList of Consultations:");
            var consultations = controller.ListConsultations();
            foreach (var consultation in consultations)
            {
                Console.WriteLine($"Appointment ID: {consultation.AppointmentID}, Notes: {consultation.Notes}, Prescription: {consultation.Prescription}");
            }
        }
    }
}