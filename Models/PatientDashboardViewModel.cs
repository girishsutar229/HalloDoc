using HalloDoc.DataModels;

namespace HalloDoc.Models
{
    public class PatientDashboardViewModel
    {

        public User User { get; set; } = new User();

        public IEnumerable<Request> RequestsData { get; set; }
    }
}
