using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BCrypt.Net;

namespace ConferenceSystem.Models
{
	public class User
	{
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [StringLength(50, ErrorMessage = "Only 50 characters are allowed")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [StringLength(50, ErrorMessage = "Only 50 characters are allowed")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<UserRole> ?UserRole { get; set; }

        [NotMapped]
        [DisplayName("Full Name")]
        public string FullName => FirstName + " " + LastName;
    }
}

