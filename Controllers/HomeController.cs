using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(MyVieeModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            return BadRequest("Invalid");
        }
    }
}
