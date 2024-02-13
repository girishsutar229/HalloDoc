using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.DataModels
{
    public class BussinessRequest
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter FirstName ")]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        public string? LastName { get; set; }
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Email is not valid.")]
        [Required(ErrorMessage = "Please Enter Email ID")]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter PhoneNumber")]
        [RegularExpression(@"^\+(?:[0-9]?){6,14}[0-9]$", ErrorMessage = "Enter valid Phone number")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; } = null!;

        [StringLength(100)]
        public string? BusinessName { get; set; }

        [StringLength(50)]
        public string? CaseNumber { get; set; }

        [StringLength(500)]
        public string? PatientSymptoms { get; set; }

        [Required(ErrorMessage = "Please Enter PatientFirstName")]
        [StringLength(100)]
        public string PatientFirstName { get; set; } = null!;

        [StringLength(100)]
        public string? PatientLastName { get; set; }

        [Required(ErrorMessage = "Please Enter PatientEmail")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Email is not valid.")]
        [StringLength(50)]
        public string PatientEmail { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter PatientPhoneNumber")]
        [RegularExpression(@"^\+(?:[0-9]?){6,14}[0-9]$", ErrorMessage = "Enter valid PatientPhoneNumber")]
        [StringLength(20)]
        public string? PatientPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter PatientStreet")]
        [StringLength(100)]
        public string PatientStreet { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter PatientCity")]
        [StringLength(100)]
        public string PatientCity { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter PatientState")]
        [StringLength(100)]
        public string PatientState { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter PatientZipCode")]
        [StringLength(10)]
        public string PatientZipCode { get; set; } = null!;

        public DateOnly PatientDateOfBirth { get; set; }

        [StringLength(10)]
        public string? PatientRoomNumber { get; set; }
    }
}
