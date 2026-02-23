using Microsoft.AspNetCore.Mvc;
using System.Data;
using VotersListProject.Models;

namespace VotersListProject.Controllers
{
    public class DataShowController : Controller
    {

        public AddDB _db;
        public DataShowController()
        {
            _db = new AddDB();
        }
        public IActionResult ShowData(string searchString, int page = 1)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
                return RedirectToAction("Login", "Voters");

            int pageSize = 5;

            var data = _db.votertable.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(x =>
                    x.VoterName.Contains(searchString) ||
                    x.Email.Contains(searchString) ||
                    x.VoterId.Contains(searchString) ||
                    x.Aadhaar.Contains(searchString));
            }

            int totalRecords = data.Count();

            var pagedData = data
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;

            ViewBag.TotalUsers = _db.votertable.Count();
            ViewBag.TotalAdmins = _db.votertable.Count(x => x.Role == "Admin");
            ViewBag.TotalNormalUsers = _db.votertable.Count(x => x.Role == "User");

            // 🔔 Pending Request Count
            ViewBag.PendingRequests = _db.UpdateRequests
                .Count(x => x.Status == "Pending");

            return View(pagedData);
        }





        public IActionResult Delete(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
                return RedirectToAction("Login", "Voters");

            var user = _db.votertable.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                _db.votertable.Remove(user);
                _db.SaveChanges();
            }

            return RedirectToAction("ShowData");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
                return RedirectToAction("Login", "Voters");

            var user = _db.votertable.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return RedirectToAction("ShowData", "DataShow");

            return View(user);
        }
       

        [HttpPost]
        public IActionResult Edit(VoterModel model)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
                return RedirectToAction("Login", "Voters");

            var user = _db.votertable.FirstOrDefault(x => x.Id == model.Id);

            if (user != null)
            {
                user.VoterName = model.VoterName;
                user.Email = model.Email;
                user.VoterId = model.VoterId;
                user.DoB = model.DoB;
                user.Address = model.Address;
                user.Aadhaar = model.Aadhaar;

                _db.SaveChanges();
            }

            return RedirectToAction("ShowData", "DataShow");
        }


        public IActionResult ManageRequests()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
                return RedirectToAction("Login", "Voters");

            var requests = _db.UpdateRequests
                              .Where(x => x.Status == "Pending")
                              .ToList();

            return View(requests);
        }


        public IActionResult ApproveRequest(int id)
        {
            var request = _db.UpdateRequests.FirstOrDefault(x => x.Id == id);

            if (request == null)
                return RedirectToAction("ManageRequests");

            var user = _db.votertable.FirstOrDefault(x => x.Id == request.UserId);

            if (user != null)
            {
                switch (request.RequestedField)
                {
                    case "VoterName":
                        user.VoterName = request.NewValue;
                        break;
                    case "Email":
                        user.Email = request.NewValue;
                        break;
                    case "Address":
                        user.Address = request.NewValue;
                        break;
                    case "DoB":
                        user.DoB = DateTime.Parse(request.NewValue);
                        break;
                }
            }

            request.Status = "Approved";

            _db.SaveChanges();

            return RedirectToAction("ManageRequests");
        }
        public IActionResult RejectRequest(int id)
        {
            var request = _db.UpdateRequests.FirstOrDefault(x => x.Id == id);

            if (request != null)
            {
                request.Status = "Rejected";
                _db.SaveChanges();
            }

            return RedirectToAction("ManageRequests");
        }




    }
}
