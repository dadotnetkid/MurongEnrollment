using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    [Authorize]
    public class SchoolYearController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult SchoolYearPartial()
        {
            var SchoolYearId = Request.Params["SchoolYearId"];
            if (SchoolYearId!="" && SchoolYearId!=null)
            {
               foreach(var i in unitOfWork.SchoolYearRepo.Get())
                {

                    i.isActive = false;
                    if (i.Id == SchoolYearId)
                        i.isActive = true;
                }
                unitOfWork.Save();
            }
                
                var model = unitOfWork.SchoolYearRepo.Get();
            return PartialView("_SchoolYearPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SchoolYearPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.SchoolYears item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    unitOfWork.SchoolYearRepo.Get().ToList().ForEach(x => x.isActive = false);
                    item.Id = Guid.NewGuid().ToString();
                    item.isActive = true;
                    unitOfWork.SchoolYearRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.SchoolYearRepo.Get();
            return PartialView("_SchoolYearPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SchoolYearPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.SchoolYears item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.SchoolYearRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.SchoolYearRepo.Get();
            return PartialView("_SchoolYearPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SchoolYearPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.SchoolYears item)
        {

            if (item != null)
            {
                try
                {
                    unitOfWork.SchoolYearRepo.Delete(unitOfWork.SchoolYearRepo.Find(m=>m.Id==item.Id));
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.SchoolYearRepo.Get();
            return PartialView("_SchoolYearPartial", model);
        }
    }
}