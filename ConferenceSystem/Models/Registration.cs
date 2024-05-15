using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace ConferenceSystem.Models
{
	public class Registration
	{
        public int RegistrationId { get; set; }


        [Required(ErrorMessage = "Please enter your full name")]
        [StringLength(100, ErrorMessage = "Only 100 characters are allowed")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }


        public int EventId { get; set; }

        public virtual EventSeminar ?Event { get; set; }
    }
}

