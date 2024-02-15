using HalloDoc.DataContext;
using HalloDoc.DataModels;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;



namespace HalloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/")]
        [Route("Patient", Name = "PatientSite")]
        public IActionResult PatientSite()
        {
            return View();
        }

        /*---------------------------------------------------------------Patients Register Page---------------------------------------------------------------------------------*/

        [Route("Patient/Register", Name = "PatientRegisterPage")]
        public IActionResult PatientRegisterPage()
        {
            return View();
            //return RedirectToAction("PatientLoginPage", "Patient");
        }
        /*--------------------------------------------------------------- Patient  ResetePSW ---------------------------------------------------------------------------------*/


        [Route("Patient/ResetePatientpsw", Name = "ResetePatientpsw")]
        public IActionResult ResetePatientpsw()
        {
            return View();
        }

        /*----------------------------------------------------------------------Patient Login Page--------------------------------------------------------------------------*/
        [HttpGet]
        [Route("Patient/Login", Name = "PatientLoginPage")]
        public IActionResult PatientLoginPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Patient/Login", Name = "PatientLoginPage")]
        public IActionResult PatientLoginPage(AspNetUsersLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                AspNetUser aspNetUserFromDb = _context.AspNetUsers.FirstOrDefault(a => a.Email == model.Email);

                if (aspNetUserFromDb != null && aspNetUserFromDb.PasswordHash == model.PasswordHash)
                {
                    //Cookie

                    //var userFromDb = _context.Users.FirstOrDefault(b => b.AspNetUserId == aspNetUserFromDb.Id);
                    //CookieOptions cookieOption = new CookieOptions();
                    //cookieOption.Secure = true;
                    //cookieOption.Expires = DateTime.Now.AddMinutes(30);
                    //Response.Cookies.Append("UserID", userFromDb.UserId.ToString(), cookieOption);

                    //sessions

                    var userFromDb = _context.Users.OrderBy(u => u.UserId).LastOrDefault(u => u.Email == model.Email);
                    HttpContext.Session.SetString("Email", userFromDb.Email);

                    TempData["success"] = "User LogIn Successfully";
                    return RedirectToAction("PatientDashboard", "Patient");
                }
                else if (aspNetUserFromDb == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email address");
                    TempData["error"] = "Invalid Email address";
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email OR password");
                    TempData["error"] = "Invalid PassWord";
                    return View(model);
                }

            }
            return View(model);
        }


        /*--------------------------------------------------------------- Patient LogOut Page---------------------------------------------------------------------------------*/

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            return View("Patientlogin");
        }

        /*----------------------------------------------------------------   PatientDashBoard  ------------------------------------------------------------------------------------*/


        public IActionResult PatientDashboard()
        {

            //int userID = int.Parse(Request.Cookies["UserID"]);
            //PatientDashboardViewModel dashboardData = new PatientDashboardViewModel();
            //dashboardData.User = _context.Users.FirstOrDefault(a => a.UserId == userID);
            //dashboardData.RequestsData = _context.Requests.Where(b => b.UserId == userID).ToList();
            //return View(dashboardData);

            var email = HttpContext.Session.GetString("Email");
            if (email == null)
            {
                return RedirectToAction("PatientLoginPage", "Patient");
            }
            List<Request> data = _context.Requests.Where(u => u.Email == email).ToList();
            Dictionary<int, int> requestIdCounts = new Dictionary<int, int>();
            foreach (var request in data)
            {
                int count = _context.RequestWiseFiles.Count(r => r.RequestId == request.RequestId);
                requestIdCounts.Add(request.RequestId, count);
            }

            ViewBag.RequestIdCounts = requestIdCounts;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            ViewBag.username = user?.FirstName + " " + user?.LastName;
            ViewBag.userid = user?.UserId;
            HttpContext.Session.SetInt32("userid", user.UserId);
            ViewBag.file = _context.RequestWiseFiles.ToList();
            return View(data);
        }

        /*-----------------------------------------------------Patient View DocumentList and Downloaded---------------------------------------------------------------------------------*/

        [Route("Patient/PatientDashboard/Viewdocuments/{id}")]        public IActionResult ViewDocuments(string id)        {

            var email = HttpContext.Session.GetString("Email");
            if (email == null)
            {
                return View("Dashboard");
            }
            ViewBag.requestId = id;
            ViewBag.file = _context.RequestWiseFiles.Where(d => d.RequestId.ToString() == id).ToList();
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            ViewBag.name = user?.LastName + " " + user?.FirstName;
            return View();        }        public IActionResult Download(int documentid)        {            var filename = _context.RequestWiseFiles.FirstOrDefault(u => u.RequestWiseFileId == documentid);
            //  var filepath = Path.Combine(hostingEnvironment.WebRootPath, "uploads", filename.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/patient-uploads", filename.FileName);

            return File(System.IO.File.ReadAllBytes(filePath), "multipart/form-data", System.IO.Path.GetFileName(filePath));
        }

        /*---------------------------------------------------------------Patientdashboard Request OR  ME&someOneElse---------------------------------------------------------------------------------*/

        [Route("Patient/Login/PatientDashboard/RequestMe", Name = "RequestMe")]
        public IActionResult RequestMe()
        {
            return View();
        }
        [Route("Patient/Login/PatientDashboard/RequestSomeOne", Name = "RequestSomeOne")]
        public IActionResult RequestSomeOne()
        {
            return View();
        }


        /*--------------------------------------------------------------- Submit TypesOfRequest ---------------------------------------------------------------------------------*/

        [Route("Patient/RequestTypes", Name = "TypesOfSubmitRequest")]
        public IActionResult TypesOfSubmitRequest()
        {
            return View();

        }
        /*--------------------------------------------------------------- PatientsTypeRequest---------------------------------------------------------------------------------*/
        [HttpGet]
        [Route("Patient/RequestTypes/Patient", Name = "PatientsTypeRequest")]
        public IActionResult PatientsTypeRequest()
        {

            return View();


        }

        [HttpPost]
        [Route("Patient/RequestTypes/Patient", Name = "PatientsTypeRequest")]
        public async Task<IActionResult> PatientsTypeRequest(PatientRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var firsttwocharsfromfname = model.FirstName.Substring(0, 2);
                var lasttwocharsfromlname = model.LastName.Substring(0, 2);
                var stateabbr = model.State.Substring(0, 2);
                var date = DateTime.Now.Day.ToString("00");
                var month = DateTime.Now.Month.ToString("00");
                var totalRequests = _context.Requests.Where(r => r.CreatedDate.Date == DateTime.Now.Date).Count().ToString("0000");

                var aspnetuser = _context.AspNetUsers.Any(u => u.Email == model.Email);
                if (!aspnetuser)
                {
                    var aspnetuser1 = new AspNetUser
                    {
                        UserName = model.FirstName,
                        Email = model.Email,
                        PasswordHash = model.PasswordHash,
                        CreatedDate = DateTime.Now,
                        PhoneNumber = model.PhoneNumber,
                    };

                    _context.AspNetUsers.Add(aspnetuser1);
                    await _context.SaveChangesAsync();

                    var user = new User
                    {
                        AspNetUserId = aspnetuser1.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Mobile = model.PhoneNumber,
                        ZipCode = model.ZipCode,
                        State = model.State,
                        City = model.City,
                        Street = model.Street,
                        CreatedBy = "patient",
                        CreatedDate = DateTime.Now,
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email); //Existing user cheking for userId
                Request request = new Request
                {
                    UserId = existingUser?.UserId,
                    RequestTypeId = 1,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    CreatedDate = DateTime.Now,
                    Status = 1,
                    ConfirmationNumber = (stateabbr + date + month + lasttwocharsfromlname + firsttwocharsfromfname + totalRequests).ToUpper(),
                };

                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                var Request = _context.Requests.FirstOrDefault(u => u.Email == model.Email);
                var requestClient = new RequestClient
                {
                    RequestId = Request.RequestId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    IntDate = model.BirthDate.Day,
                    IntYear = model.BirthDate.Year,
                    StrMonth = model.BirthDate.ToString("MMMM"),
                    ZipCode = model.ZipCode,
                    State = model.State,
                    City = model.City,
                    Street = model.Street,
                    Address = model.PatientRoomNumber + " ," + model.City + " ," + model.State + " ," + model.ZipCode,

                };

                _context.RequestClients.Add(requestClient);
                await _context.SaveChangesAsync();

                //For File Store
                if (model.formFile != null && model.formFile.Count > 0)
                {
                    foreach (var file in model.formFile)
                    {
                        var fileId = _context.RequestWiseFiles.Count() + 1;
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFiles", ("(" + fileId.ToString() + ")") + file.FileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(stream);

                            var userCheck = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(e => e.Email == model.Email);
                            RequestWiseFile requestwiseFile = new RequestWiseFile();
                            requestwiseFile.RequestId = userCheck.RequestId;
                            requestwiseFile.FileName = file.FileName;
                            requestwiseFile.CreatedDate = DateTime.Now;
                            requestwiseFile.DocType = 1;

                            _context.RequestWiseFiles.Add(requestwiseFile);
                            _context.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("PatientSite", "Patient");
            }
            return View(model);
        }

        /*-----------------------------------------------------FamilyTypeRequest---------------------------------------------------------------------------------*/

        [HttpGet]
        [Route("Patient/RequestTypes/Family", Name = "FamilyTypeRequest")]
        public IActionResult FamilyTypeRequest()
        {
            return View();
        }

        [HttpPost]
        [Route("Patient/RequestTypes/Family", Name = "FamilyTypeRequest")]
        public async Task<IActionResult> FamilyTypeRequest(FamilyFriendRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var firsttwocharsfromfname = model.PatientFirstName.Substring(0, 2);
                var lasttwocharsfromlname = model.PatientLastName.Substring(0, 2);
                var stateabbr = model.PatientState.Substring(0, 2);
                var date = DateTime.Now.Day.ToString("00");
                var month = DateTime.Now.Month.ToString("00");
                var totalRequests = _context.Requests.Where(r => r.CreatedDate.Date == DateTime.Now.Date).Count().ToString("0000");

                var exitinguser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                var request = new Request
                {
                    UserId = exitinguser?.UserId,
                    RequestTypeId = 2,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    CreatedDate = DateTime.Now,
                    RelationName = model.RelationName,
                    Status = 1,
                    ConfirmationNumber = (stateabbr + date + month + lasttwocharsfromlname + firsttwocharsfromfname + totalRequests).ToUpper(),
                };
                _context.Requests.Add(request);

                await _context.SaveChangesAsync();

                var requestCheck = _context.Requests.FirstOrDefault(u => u.Email == model.Email);
                var requestclient = new RequestClient
                {
                    RequestId = requestCheck.RequestId,
                    FirstName = model.PatientFirstName,
                    LastName = model.PatientLastName,
                    PhoneNumber = Convert.ToString(model.PatientPhoneNumber),
                    Email = model.PatientEmail,
                    IntDate = model.PatientDateOfBirth.Day,
                    IntYear = model.PatientDateOfBirth.Year,
                    StrMonth = model.PatientDateOfBirth.ToString("MMMM"),
                    ZipCode = model.PatientZipCode,
                    State = model.PatientState,
                    City = model.PatientCity,
                    Street = model.PatientStreet,
                    Address = model.PatientRoomNumber + " ," + model.PatientCity + " ," + model.PatientState + " ," + model.PatientZipCode,

                };
                _context.RequestClients.Add(requestclient);
                await _context.SaveChangesAsync();

                //For File Store
                if (model.formFile != null && model.formFile.Count > 0)
                {
                    foreach (var file in model.formFile)
                    {
                        var fileId = _context.RequestWiseFiles.Count() + 1;
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFiles", ("(" + fileId.ToString() + ")") + file.FileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(stream);

                            var userCheck = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(e => e.Email == model.Email);
                            RequestWiseFile requestwiseFile = new RequestWiseFile();
                            requestwiseFile.RequestId = userCheck.RequestId;
                            requestwiseFile.FileName = file.FileName;
                            requestwiseFile.CreatedDate = DateTime.Now;
                            requestwiseFile.DocType = 1;

                            _context.RequestWiseFiles.Add(requestwiseFile);
                            _context.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("PatientSite", "Patient");

            }

            return View();

        }

        /*-----------------------------------------------------ConciergeTypeRequest---------------------------------------------------------------------------------*/
        [HttpGet]
        [Route("Patient/RequestTypes/Concierge", Name = "ConciergeTypeRequest")]
        public IActionResult ConciergeTypeRequest()
        {
            return View();
        }

        [HttpPost]
        [Route("Patient/RequestTypes/Concierge", Name = "ConciergeTypeRequest")]
        public async Task<IActionResult> ConciergeTypeRequest(ConciergeRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var firsttwocharsfromfname = model.PatientFirstName.Substring(0, 2);
                var lasttwocharsfromlname = model.PatientLastName.Substring(0, 2);
                var stateabbr = model.State.Substring(0, 2);
                var date = DateTime.Now.Day.ToString("00");
                var month = DateTime.Now.Month.ToString("00");
                var totalRequests = _context.Requests.Where(r => r.CreatedDate.Date == DateTime.Now.Date).Count().ToString("0000");

                var userCheck = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                var newRequest = new Request
                {
                    RequestTypeId = 3,
                    UserId = userCheck?.UserId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    Email = model.Email,
                    //PropertyName = model.PropertyName,
                    Status = 1,
                    ConfirmationNumber = (stateabbr + date + month + lasttwocharsfromlname + firsttwocharsfromfname + totalRequests).ToUpper(),
                };

                _context.Requests.Add(newRequest);
                await _context.SaveChangesAsync();

                var requestCheck = _context.Requests.FirstOrDefault(u => u.Email == model.Email);

                var newRequestClient = new RequestClient
                {
                    RequestId = requestCheck.RequestId,
                    FirstName = model.PatientFirstName,
                    LastName = model.PatientLastName,
                    PhoneNumber = model.PatientPhoneNumber,
                    Email = model.PatientEmail,
                    //DateOfBirth = model.PatientDateOfBirth,
                    IntDate = model.PatientDateOfBirth.Day,
                    IntYear = model.PatientDateOfBirth.Year,
                    StrMonth = model.PatientDateOfBirth.ToString("MMMM"),
                    Address = model.PatientRoomNumber + " ," + model.Street + " ," + model.City + " ," + model.State + " ," + model.ZipCode,
                };

                _context.RequestClients.Add(newRequestClient);
                await _context.SaveChangesAsync();
                return RedirectToAction("PatientSite", "Patient");

            }
            return View();

        }

        /*-----------------------------------------------------BussinesTypeRequest---------------------------------------------------------------------------------*/
        [HttpGet]

        [Route("Patient/RequestTypes/Bussines", Name = "BussinesTypeRequest")]
        public IActionResult BussinesTypeRequest()
        {
            return View();
        }

        [HttpPost]
        [Route("Patient/RequestTypes/Bussines", Name = "BussinesTypeRequest")]
        public async Task<IActionResult> BussinesTypeRequest(BussinessRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var firsttwocharsfromfname = model.PatientFirstName.Substring(0, 2);
                var lasttwocharsfromlname = model.PatientLastName.Substring(0, 2);
                var stateabbr = model.PatientState.Substring(0, 2);
                var date = DateTime.Now.Day.ToString("00");
                var month = DateTime.Now.Month.ToString("00");
                var totalRequests = _context.Requests.Where(r => r.CreatedDate.Date == DateTime.Now.Date).Count().ToString("0000");

                var userCheck = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                var newRequest = new Request
                {
                    RequestTypeId = 4,
                    UserId = userCheck?.UserId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    Email = model.Email,
                    CaseNumber = model.CaseNumber,
                    //PropertyName = model.BussinessName,
                    Status = 1,
                    ConfirmationNumber = (stateabbr + date + month + lasttwocharsfromlname + firsttwocharsfromfname + totalRequests).ToUpper(),
                };
                _context.Requests.Add(newRequest);
                await _context.SaveChangesAsync();

                var requestCheck = _context.Requests.FirstOrDefault(u => u.Email == model.Email);

                var newRequestClient = new RequestClient
                {
                    RequestId = requestCheck.RequestId,
                    FirstName = model.PatientFirstName,
                    LastName = model.PatientLastName,
                    PhoneNumber = model.PatientPhoneNumber,
                    Email = model.PatientEmail,
                    //DateOfBirth = model.PatientDateOfBirth,
                    IntDate = model.PatientDateOfBirth.Day,
                    IntYear = model.PatientDateOfBirth.Year,
                    StrMonth = model.PatientDateOfBirth.ToString("MMMM"),
                    ZipCode = model.PatientZipCode,
                    State = model.PatientState,
                    City = model.PatientCity,
                    Street = model.PatientStreet,
                    Address = model.PatientRoomNumber + " ," + model.PatientCity + " ," + model.PatientState + " ," + model.PatientZipCode,
                };

                _context.RequestClients.Add(newRequestClient);
                await _context.SaveChangesAsync();
                return RedirectToAction("PatientSite", "Patient");
            }

            return View(model);

        }
        /*-----------------------------------------------------SubmitRequestForMe---------------------------------------------------------------------------------*/
        public IActionResult SubmitRequestForMe()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRequestForMe(PatientRequestViewModel model)
        {

            if (!ModelState.IsValid)
            {
                var uid = HttpContext.Session.GetInt32("userid");
                var newRequest = new Request
                {
                    RequestTypeId = 1,
                    UserId = uid,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    Email = model.Email,
                    Status = 1,

                };

                _context.Requests.Add(newRequest);
                await _context.SaveChangesAsync();

                var requestCheck = _context.Requests.FirstOrDefault(r => r.Email == model.Email);

                var newClientRequest = new RequestClient
                {
                    RequestId = requestCheck.RequestId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    IntDate = model.BirthDate.Day,
                    IntYear = model.BirthDate.Year,
                    StrMonth = model.BirthDate.ToString("MMMM"),
                    ZipCode = model.ZipCode,
                    State = model.State,
                    City = model.City,
                    Street = model.Street,
                    Address = model.PatientRoomNumber + " ," + model.City + " ," + model.State + " ," + model.ZipCode,

                };
                _context.RequestClients.Add(newClientRequest);
                await _context.SaveChangesAsync();

                //For File Store
                if (model.formFile != null && model.formFile.Count > 0)
                {
                    foreach (var file in model.formFile)
                    {
                        var fileId = _context.RequestWiseFiles.Count() + 1;
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFiles", ("(" + fileId.ToString() + ")") + file.FileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(stream);

                            var userCheck = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(e => e.Email == model.Email);
                            RequestWiseFile requestwiseFile = new RequestWiseFile();
                            requestwiseFile.RequestId = userCheck.RequestId;
                            requestwiseFile.FileName = file.FileName;
                            requestwiseFile.CreatedDate = DateTime.Now;
                            requestwiseFile.DocType = 1;

                            _context.RequestWiseFiles.Add(requestwiseFile);
                            _context.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("PatientDashboard", "Patient");
            }
            return View(model);
        }


        public IActionResult SubmitRequestForSomeOne()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitRequestForSomeone(FamilyFriendRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var firsttwocharsfromfname = model.PatientFirstName.Substring(0, 2);
                var lasttwocharsfromlname = model.PatientLastName.Substring(0, 2);
                var stateabbr = model.PatientState.Substring(0, 2);
                var date = DateTime.Now.Day.ToString("00");
                var month = DateTime.Now.Month.ToString("00");
                var totalRequests = _context.Requests.Where(r => r.CreatedDate.Date == DateTime.Now.Date).Count().ToString("0000");

                var exitinguser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                var request = new Request
                {
                    UserId = exitinguser?.UserId,
                    RequestTypeId = 2,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    CreatedDate = DateTime.Now,
                    RelationName = model.RelationName,
                    Status = 1,
                    ConfirmationNumber = (stateabbr + date + month + lasttwocharsfromlname + firsttwocharsfromfname + totalRequests).ToUpper(),
                };
                _context.Requests.Add(request);

                await _context.SaveChangesAsync();

                var requestCheck = _context.Requests.FirstOrDefault(u => u.Email == model.Email);
                var requestclient = new RequestClient
                {
                    RequestId = requestCheck.RequestId,
                    FirstName = model.PatientFirstName,
                    LastName = model.PatientLastName,
                    PhoneNumber = Convert.ToString(model.PatientPhoneNumber),
                    Email = model.PatientEmail,
                    IntDate = model.PatientDateOfBirth.Day,
                    IntYear = model.PatientDateOfBirth.Year,
                    StrMonth = model.PatientDateOfBirth.ToString("MMMM"),
                    ZipCode = model.PatientZipCode,
                    State = model.PatientState,
                    City = model.PatientCity,
                    Street = model.PatientStreet,
                    Address = model.PatientRoomNumber + " ," + model.PatientCity + " ," + model.PatientState + " ," + model.PatientZipCode,

                };
                _context.RequestClients.Add(requestclient);
                await _context.SaveChangesAsync();

                //For File Store
                if (model.formFile != null && model.formFile.Count > 0)
                {
                    foreach (var file in model.formFile)
                    {
                        var fileId = _context.RequestWiseFiles.Count() + 1;
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFiles", ("(" + fileId.ToString() + ")") + file.FileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(stream);

                            var userCheck = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(e => e.Email == model.Email);
                            RequestWiseFile requestwiseFile = new RequestWiseFile();
                            requestwiseFile.RequestId = userCheck.RequestId;
                            requestwiseFile.FileName = file.FileName;
                            requestwiseFile.CreatedDate = DateTime.Now;
                            requestwiseFile.DocType = 1;

                            _context.RequestWiseFiles.Add(requestwiseFile);
                            _context.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("PatientDashboard", "Patient");

            }

            return View(model);

        }


        /*-----------------------------------------------------ErrorViewModel---------------------------------------------------------------------------------*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

   

}




