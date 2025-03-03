using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Notification2.Models; // Ensure this namespace is correct

namespace Notification2.Models
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