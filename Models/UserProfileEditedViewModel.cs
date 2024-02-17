using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Models
{
    public class UserProfileEditedViewModel
    {
   
        [Required(ErrorMessage = "Please enter a First Name")]
        [RegularExpression(@"([a-zA-Z]*)", ErrorMessage = "Please enter only alphabets.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a Last Name")]
        [RegularExpression(@"([a-zA-Z]*)", ErrorMessage = "Please enter only alphabets.")]
        public string LastName { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter a Mobile Number.")]
        //[RegularExpression(@"([0-9]*)", ErrorMessage = "Please enter only numbers.")]
        public string Mobile { get; set; } = null!;


        [Required(ErrorMessage = "Please enter an Email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Please enter a valid Street name")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a valid City name")]
        [RegularExpression(@"([a-zA-Z]*)", ErrorMessage = "Please enter only alphabets.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a valid State name")]
        [RegularExpression(@"([a-zA-Z]*)", ErrorMessage = "Please enter only alphabets.")]
        public string State { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a valid ZipCode")]
        [StringLength(6, ErrorMessage = "Zipcode must be length of 6.")]
        public string ZipCode { get; set; } = null!;


        [Precision(9, 5)]
        public decimal? Latitude { get; set; }

        [Precision(9, 5)]
        public decimal? Longitude { get; set; }


    }
}
