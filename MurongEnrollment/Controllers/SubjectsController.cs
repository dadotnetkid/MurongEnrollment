using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    [Authorize]
    public class SubjectsController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Subjects
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var model = unitOfWork.SubjectRepo.Get();
            return PartialView("_GridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Subjects item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    item.Id = Guid.NewGuid().ToString();
                    unitOfWork.SubjectRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.SubjectRepo.Get();
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Subjects item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.SubjectRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.SubjectRepo.Get();
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Subjects item)
        {

            if (item != null)
            {
                try
                {
                    var subject = unitOfWork.SubjectRepo.Find(m => m.Id == item.Id);
                    unitOfWork.SubjectRepo.Delete(subject);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.SubjectRepo.Get();
            return PartialView("_GridViewPartial", model);
        }
    }
}