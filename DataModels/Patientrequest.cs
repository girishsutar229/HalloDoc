using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace HalloDoc.DataModels
{
    public class PatientRequest
    {
        [Key]
        public string Symptoms { get; set; }

        [Required(ErrorMessage = "Please Enter FirstName")]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter FirstName")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email ID")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Email is not valid.")]
        //[EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter PhoneNumber")]
        [RegularExpression(@"^\+(?:[0-9]?){6,14}[0-9]$", ErrorMessage = "Enter valid Phone number")]
        [StringLength(23)]
        public string? PhoneNumber { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage ="Please the EnterValid password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MaxLength(255)] // Adjust the max length as per your hashing algorithm
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("PasswordHash", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [NotMapped] // This property won't be mapped to the database
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter a Street name")]
        [StringLength(100)]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Please enter a City name")]
        [StringLength(100)]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a State name")]
        [StringLength(100)]
        public string State { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a Zip Code")]
        [StringLength(10)]
        public string ZipCode { get; set; } = null!;

        [Required(ErrorMessage = "Please provide a Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [StringLength(50)]
        public string? PatientRoomNumber { get; set; }
    }
}