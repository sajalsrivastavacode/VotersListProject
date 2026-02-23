using Microsoft.AspNetCore.Mvc;
using VotersListProject.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VotersListProject.Controllers
{
    public class VotersController : Controller
    {
        public AddDB _db;

        public VotersController()
        {

            _db = new AddDB();
        }
        [HttpGet]
        public IActionResult AddVoters()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddVoters(VoterModel obj)
        {
            if (ModelState.IsValid)
            {

                _db.votertable.Add(obj);
                _db.SaveChanges();

                return RedirectToAction("ShowData", "DataShow");

            }
            // db
            return View(obj);

        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        //[HttpGet]
        //public IActionResult Login()
        //{
        //    Random rnd = new Random();
        //    int num1 = rnd.Next(1, 10);
        //    int num2 = rnd.Next(1, 10);

        //    HttpContext.Session.SetInt32("CaptchaAnswer", num1 + num2);

        //    ViewBag.Num1 = num1;
        //    ViewBag.Num2 = num2;

        //    return View();
        //}


        [HttpPost]
        public IActionResult Login(VoterModel obj)
        {
            var user = _db.votertable
                .FirstOrDefault(x => x.Email == obj.Email
                                  && x.Password == obj.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.VoterName);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("ShowData", "DataShow");
                }
                else
                {
                    return RedirectToAction("SearchUser");
                }
            }

            ViewBag.Error = "Invalid Email or Password";
            return View();
        }



        //[HttpPost]
        //public IActionResult Login(VoterModel obj, int captchaInput)
        //{
        //    int? correctAnswer = HttpContext.Session.GetInt32("CaptchaAnswer");

        //    if (correctAnswer == null || captchaInput != correctAnswer)
        //    {
        //        ViewBag.Error = "Invalid Captcha";
        //        return RedirectToAction("Login");
        //    }

        //    var user = _db.votertable
        //        .FirstOrDefault(x => x.Email == obj.Email
        //                          && x.Password == obj.Password);

        //    if (user != null)
        //    {
        //        HttpContext.Session.SetString("UserRole", user.Role);
        //        HttpContext.Session.SetInt32("UserId", user.Id);
        //        HttpContext.Session.SetString("UserName", user.VoterName);

        //        if (user.Role == "Admin")
        //            return RedirectToAction("ShowData", "DataShow");
        //        else
        //            return RedirectToAction("SearchUser");
        //    }

        //    ViewBag.Error = "Invalid Email or Password";
        //    return View();
        //}




        [SetSessionGlobally]
        public IActionResult SearchUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchUser(string email)
        {
            var user = _db.votertable
                          .FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                ViewBag.Error = "User not found";
                return View();
            }

            return View("UserResult", user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [SetSessionGlobally]

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login");

            var user = _db.votertable.FirstOrDefault(x => x.Id == userId);

            if (user == null)
                return RedirectToAction("Login");

            if (user.Password != oldPassword)
            {
                ViewBag.Error = "Old password is incorrect";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "New passwords do not match";
                return View();
            }

            user.Password = newPassword;
            _db.SaveChanges();

            ViewBag.Success = "Password changed successfully";

            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View();
            }

            var user = _db.votertable.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                ViewBag.Error = "Email not found";
                return View();
            }

            user.Password = newPassword;
            _db.SaveChanges();

            ViewBag.Success = "Password reset successfully!";
            return View();
        }


        [SetSessionGlobally]
        [HttpGet]
        public IActionResult RequestUpdate()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "User")
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public IActionResult RequestUpdate(string RequestedField, string NewValue)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login");

            UpdateRequest request = new UpdateRequest
            {
                UserId = userId.Value,
                RequestedField = RequestedField,
                NewValue = NewValue
            };

            _db.UpdateRequests.Add(request);
            _db.SaveChanges();

            ViewBag.Success = "Your request has been sent to Admin.";

            return View();
        }







    }
}
