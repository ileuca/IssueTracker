using IssueTracker.Models;
using IssueTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository userRepository = new UserRepository();
        public ActionResult Edit()
        {
            UserModel userModel = userRepository.GetUserById(userRepository.GetCurrentUser().UserId);

            return View(userModel);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            UserModel userModel = userRepository.GetUserById(userRepository.GetCurrentUser().UserId);
            try
            {
                UpdateModel(userModel);
                userRepository.SaveUserChanges(userModel);
                return RedirectToAction("Index","Manage");
            }
            catch
            {
                return View();
            }
        }
    }
}
