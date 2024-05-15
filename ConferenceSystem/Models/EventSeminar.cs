using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConferenceSystem.Models
{
    public class EventSeminar
    {
        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Please enter the event name")]
        [StringLength(50, ErrorMessage = "Only 50 characters are allowed")]
        [DisplayName("Event Name")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Please enter the registration fees")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid registration fees")]
        [DisplayName("Registration Fees")]
        public decimal RegistrationFees { get; set; }

        [Required(ErrorMessage = "Please enter the maximum attendees")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid maximum attendees")]
        [DisplayName("Maximum Attendees")]
        public int MaxAttendees { get; set; }

        [Required(ErrorMessage = "Please enter the event time")]
        [DisplayName("Event Time")]
        public DateTime EventTime { get; set; }

        [Required(ErrorMessage = "Please enter the event location")]
        [StringLength(100, ErrorMessage = "Only 100 characters are allowed")]
        [DisplayName("Event Location")]
        public string EventLocation { get; set; }

        [Required(ErrorMessage = "Please specify the FirstName")]
        [DisplayName("Created By")]
        public int CreatedByUserUserId { get; set; }

        public virtual User? CreatedByUser { get; set; }

        public virtual ICollection<Registration>? Registration { get; set; }
    }
}
