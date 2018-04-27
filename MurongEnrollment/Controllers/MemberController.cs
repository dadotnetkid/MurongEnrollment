using DevExpress.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MurongEnrollment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    public class MemberController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        #region Identity
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult MembersGridViewPartial()
        {
            var model = unitOfWork.UserRepository.Get(includeProperties: "AspNetRoles,AspNetUserDetails");
            return PartialView("_MembersGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> MembersGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.AspNetUsers item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    item.Id = Guid.NewGuid().ToString();
                    item.Email = item.UserName;
                    var res = await UserManager.CreateAsync(new Models.Users() { Id = item.Id, Email = item.UserName, UserName = item.UserName }, item.Password);
                    if (res.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(item.Id, item.UserRoles);
                        unitOfWork.AspNetUserDetailsRepo.Insert(new Models.AspNetUserDetails() { UserId = item.Id, MiddleName = item.AspNetUserDetails.MiddleName, LastName = item.AspNetUserDetails.LastName, FirstName = item.AspNetUserDetails.FirstName });
                        unitOfWork.Save();
                    }


                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.UserRepository.Get(includeProperties: "AspNetRoles,AspNetUserDetails");
            return PartialView("_MembersGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> MembersGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.AspNetUsers item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await UserManager.FindByIdAsync(item.Id);
                    user.UserName = item.UserName;
                    user.Email = item.UserName;


                    await UserManager.UpdateAsync(user);
                    var roles = await UserManager.GetRolesAsync(user.Id);
                    await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                    await UserManager.AddToRoleAsync(user.Id, item.UserRoles);
                    if (item.Password != null && item.Password != "")
                        await UserManager.ChangePasswordAsync(user, item.Password);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.UserRepository.Get(includeProperties: "AspNetRoles,AspNetUserDetails");
            return PartialView("_MembersGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult MembersGridViewPartialDelete(System.String Id)
        {
            var model = new object[0];
            if (Id != null)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_MembersGridViewPartial", model);
        }
        public ActionResult AddEditUserPartial(string UserId = "")
        {
            var model = unitOfWork.UserRepository.Find(m => m.Id == UserId);
            return PartialView("_AddEditUserPartial", model);
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            var res = await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            return RedirectToAction("Index", "Students");
        }
        [Route("logout")]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Students");
        }
    }
}