using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace HalloDoc.DataModels
{
    public class ConciergeRequest
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Please Enter a valid First Name")]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Please Enter Email ID")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage ="Please Etheter PhoneNumber ")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? PropertyName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Please Enter a Street name")]
        public string Street { get; set; } = null!;


        [StringLength(100)]
        [Required(ErrorMessage = "Please Enter a City name")]
        public string City { get; set; } = null!;

        [StringLength(100)]
        [Required(ErrorMessage = "Please Enter a State name")]
        public string State { get; set; } = null!;

        [StringLength(10)]
        [Required(ErrorMessage = "Please Enter a ZipCode")]
        public string ZipCode { get; set; } = null!;

        [StringLength(100)]
        [Required(ErrorMessage = "Please Enter a First Name")]
        public string PatientFirstName { get; set; } = null!;

        [StringLength(100)]
        public string? PatientLastName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Please Enter Patient Email ID")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "email is not valid.")]
        public string PatientEmail { get; set; } = null!;

        [StringLength(20)]
        public string? PatientPhoneNumber { get; set; }

        [StringLength(10)]
        public string? PatientRoomNumber { get; set; }

        public DateOnly PatientDateOfBirth { get; set; }

        [StringLength(500)]
        public string? PatientSymptoms { get; set; }
    }

}

