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
    public class DocAvailability
    {
        [Key]
        public int SessionID { get; set; }
        [Required]
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public TimeOnly TimeSlot { get; set; }
        // Navigation property
        public virtual User Doctor { get; set; }
    }
}
