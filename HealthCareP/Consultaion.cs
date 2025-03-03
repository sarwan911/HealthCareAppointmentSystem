using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareAppointmentSystem
{
    public class Consultation
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ConsultationID { get; set; }

        [Required]
        public int AppointmentID { get; set; }

        [Required]
        public string Notes { get; set; }

        [Required]
        public string Prescription { get; set; }
    }
}