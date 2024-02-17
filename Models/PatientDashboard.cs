using HalloDoc.DataModels;

namespace HalloDoc.Models
{
    public class PatientDashboard
    {
        public RequestClient RequestClient { get; set; } = new RequestClient();

        public IEnumerable<Request>? RequestData { get; set; }
    }
}
