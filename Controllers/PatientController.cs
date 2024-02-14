using HalloDoc.DataContext;
using HalloDoc.DataModels;
using HalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;


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
                    var userFromDb = _context.Users.FirstOrDefault(b => b.AspNetUserId == aspNetUserFromDb.Id);
                    CookieOptions cookieOption = new CookieOptions();
                    cookieOption.Secure = true;
                    cookieOption.Expires = DateTime.Now.AddMinutes(30);
                    Response.Cookies.Append("UserID", userFromDb.UserId.ToString(), cookieOption);

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

        /*---------------------------------------------------------------ResetePatientpsw---------------------------------------------------------------------------------*/


        [Route("Patient/ResetePatientpsw", Name = "ResetePatientpsw")]
        public IActionResult ResetePatientpsw()
        {
            return View();
        }

        /*-----------------------------------------------------PatientDashBoard---------------------------------------------------------------------------------*/

        [Route("Patient/Login/PatientDashboard", Name = "PatientDashboard")]
        public IActionResult PatientDashboard()
        {

            int userID = int.Parse(Request.Cookies["UserID"]);
            PatientDashboardViewModel dashboardData = new PatientDashboardViewModel();
            dashboardData.User = _context.Users.FirstOrDefault(a => a.UserId == userID);
            dashboardData.RequestsData = _context.Requests.Where(b => b.UserId == userID).ToList();
            return View(dashboardData);
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


        /*---------------------------------------------------------------SubmitRequestTypes---------------------------------------------------------------------------------*/

        [Route("Patient/RequestTypes", Name = "SubmitRequestTypes")]
        public IActionResult SubmitRequestTypes()
        {
            return View();

        }
        /*---------------------------------------------------------------PatientsTypeRequest---------------------------------------------------------------------------------*/
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

                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);

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
                    Address = model.City + " " + model.State + " " + model.ZipCode,
                    Location = model.PatientRoomNumber,
                };

                _context.RequestClients.Add(requestClient);
                await _context.SaveChangesAsync();
                //For File Store
                if (model.formFile != null && model.formFile.Count > 0)
                {
                    foreach (var file in model.formFile)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(stream);
                            var userCheck = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(e => e.Email == model.Email);
                            RequestWiseFile newFile = new RequestWiseFile
                            {
                                RequestId = userCheck.RequestId,
                                FileName = file.FileName,
                                CreatedDate = DateTime.Now,
                                DocType = 1,
                            };
                            _context.RequestWiseFiles.Add(newFile);
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
                    Address = model.PatientCity + " " + model.PatientState + " " + model.PatientZipCode,
                    Location = model.PatientRoomNumber,

                };
                _context.RequestClients.Add(requestclient);
                await _context.SaveChangesAsync();

                //For File Store
                if (model.formFile != null && model.formFile.Count > 0)
                {
                    foreach (var file in model.formFile)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(stream);
                            var userCheck = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(e => e.Email == model.Email);
                            RequestWiseFile newFile = new RequestWiseFile
                            {
                                RequestId = userCheck.RequestId,
                                FileName = file.FileName,
                                CreatedDate = DateTime.Now,
                                DocType = 1,
                            };
                            _context.RequestWiseFiles.Add(newFile);
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
            var userCheck = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (ModelState.IsValid)
            {
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
                    Address = model.Street + "" + model.City + " " + model.State + " " + model.ZipCode,
                    Location = model.PatientRoomNumber,

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
            var userCheck = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (ModelState.IsValid)
            {
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
                    Address = model.PatientCity + " " + model.PatientState + " " + model.PatientZipCode,
                    Location = model.PatientRoomNumber,
                };

                _context.RequestClients.Add(newRequestClient);
                await _context.SaveChangesAsync();
                return RedirectToAction("PatientSite", "Patient");
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



