using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DataModels
{
    public class FamilyFriendRequest
    {

        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter a FirstName ")]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage ="please Enter Phonenumber")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }


        [StringLength(100)]
        public string? RelationName { get; set; }

        [StringLength(500)]
        public string? PatientSymptoms { get; set; }

        [Required(ErrorMessage = "Please Enter a PatientFirstName")]
        [StringLength(100)]
        public string PatientFirstName { get; set; } = null!;

        [StringLength(100)]
        public string? PatientLastName { get; set; }

        [Required(ErrorMessage = "Please Enter a PatientEmail name")]
        [StringLength(50)]
        public string PatientEmail { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter a PatientPhonenumber")]
        [StringLength(20)]
        public string? PatientPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter a PatientStreet ")]
        [StringLength(100)]
        public string PatientStreet { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter a PatientCity name")]
        [StringLength(100)]
        public string PatientCity { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter a PatientState name")]
        [StringLength(100)]
        public string PatientState { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter a PatientZipCode name")]
        [StringLength(10)]
        public string PatientZipCode { get; set; } = null!;

        public DateOnly PatientDateOfBirth { get; set; }

        [StringLength(10)]
        public string? PatientRoomNumber { get; set; }

    }
}