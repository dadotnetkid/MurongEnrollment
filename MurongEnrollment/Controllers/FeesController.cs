using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    [Authorize]
    public class FeesController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult FeesGridViewPartial()
        {
            var model = unitOfWork.StandardFeeRepo.Get(includeProperties: "GradeLevels");
            return PartialView("_FeesGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FeesGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.StandardFees item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    item.Id = Guid.NewGuid().ToString();
                    unitOfWork.StandardFeeRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.StandardFeeRepo.Get(includeProperties: "GradeLevels");
            return PartialView("_FeesGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult FeesGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.StandardFees item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.StandardFeeRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.StandardFeeRepo.Get(includeProperties: "GradeLevels");
            return PartialView("_FeesGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult FeesGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.StandardFees item)
        {
            //var model = new object[0];
            if (item != null)
            {
                try
                {
                    unitOfWork.StandardFeeRepo.Delete(unitOfWork.StandardFeeRepo.Find(x => x.Id == item.Id));
                    unitOfWork.Save();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.StandardFeeRepo.Get(includeProperties: "GradeLevels");
            return PartialView("_FeesGridViewPartial", model);
        }
    }
}