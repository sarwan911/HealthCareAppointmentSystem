using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareAppointmentSystem
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime SentDate { get; set; }

        public bool IsRead { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }
    }
}