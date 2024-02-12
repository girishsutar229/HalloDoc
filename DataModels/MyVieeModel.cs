using System.ComponentModel.DataAnnotations;

namespace HalloDoc.DataModels
{
    public class MyVieeModel
    {
        [Required(ErrorMessage = "ENtdddddddddddddder")]
        public string email { get; set; } = string.Empty;
        [Required(ErrorMessage = "khsadguad")]
        [DataType(DataType.Password)]
        public string password { get; set; } = string.Empty;
    }
}
