using HalloDoc.DataModels;

namespace HalloDoc.Models
{
    public class PatientDashboardViewModel
    {
        
        public User User { get; set; } = new User();

        public UserProfileEditedViewModel ProfileEdited { get; set; } = new UserProfileEditedViewModel();

        public IEnumerable<Request> RequestsData { get; set; }
        public IEnumerable<RequestClient> RequestsClientData { get; set; }

        public List<IFormFile> formFile { get; set; }

        public string? BirthDate { get; set; }


    }
}
