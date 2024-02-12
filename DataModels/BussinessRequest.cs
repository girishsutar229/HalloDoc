using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.DataModels
{
    public class BussinessRequest
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter a FirstName ")]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please Enter a Email name")]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter a PhoneNumber name")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; } = null!;

        [StringLength(100)]
        public string? BusinessName { get; set; }

        [StringLength(50)]
        public string? CaseNumber { get; set; }

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

        [Required(ErrorMessage = "Please Enter a PatientPhoneNumber name")]
        [StringLength(20)]
        public string? PatientPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter a PatientStreet name")]
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
