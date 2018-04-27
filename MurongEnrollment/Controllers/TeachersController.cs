using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{

    [Authorize]
    public class TeachersController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult TeacherGridViewPartial()
        {
            var model = unitOfWork.TeacherRepo.Get();
            return PartialView("_TeacherGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TeacherGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Teachers item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    item.Id = Guid.NewGuid().ToString();
                    unitOfWork.TeacherRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.TeacherRepo.Get();
            return PartialView("_TeacherGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TeacherGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Teachers item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.TeacherRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.TeacherRepo.Get();
            return PartialView("_TeacherGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TeacherGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Teachers item)
        {

            if (item != null)
            {
                try
                {
                    unitOfWork.TeacherRepo.Delete(unitOfWork.TeacherRepo.Find(m => m.Id == item.Id));
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.TeacherRepo.Get();
            return PartialView("_TeacherGridViewPartial", model);
        }
    }
}