using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCareAppointmentSystem;

namespace HealthCareAppointmentSystem
{
    public class AppointmentDetails
    {
        [Key]
        public int AppointmentID { get; set; }
        [Required(ErrorMessage = "Patient ID must be given.")]
        public int PatientID { get; set; }
        [Required(ErrorMessage = "Doctor must be assigned to the patient.")]
        public int DoctorID { get; set; }
        [Required(ErrorMessage = "Assign a date to the patient.")]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        [Required(ErrorMessage = "Assign a time slot to the patient.")]
        [DataType(DataType.Time)]
        public TimeOnly TimeSlot { get; set; }
        [Required(ErrorMessage = "Assign a location to the patient.")]
        public string Location { get; set; }

        [ForeignKey(nameof(PatientID))]
        public virtual User Patient { get; set; }
        [ForeignKey(nameof(DoctorID))]
        public virtual User Doctor { get; set; }

    }
}