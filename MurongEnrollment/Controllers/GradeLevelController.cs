using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    public class GradeLevelController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        #region Grade Level
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GradeLevelGridViewPartial()
        {
            var model = unitOfWork.GradelevelRepo.Get();
            return PartialView("_GradeLevelGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GradeLevelGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.GradeLevels item)
        {
          
            if (ModelState.IsValid)
            {
                try
                {
                    item.Id = Guid.NewGuid().ToString();
                    unitOfWork.GradelevelRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.GradelevelRepo.Get();
            return PartialView("_GradeLevelGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GradeLevelGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.GradeLevels item)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.GradelevelRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.GradelevelRepo.Get();
            return PartialView("_GradeLevelGridViewPartial", model);
        }
      
        #endregion
        [Route("Sections")]
        public ActionResult Sections()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult SectionsGridViewPartial()
        {
            var model = unitOfWork.SectionRepo.Get();
            return PartialView("_SectionsGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SectionsGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Sections item)
        {
     
            if (ModelState.IsValid)
            {
                try
                {
                    item.Id = Guid.NewGuid().ToString();
                    unitOfWork.SectionRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.SectionRepo.Get();
            return PartialView("_SectionsGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SectionsGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Sections item)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.SectionRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.SectionRepo.Get();
            return PartialView("_SectionsGridViewPartial", model);
        }
      
    }
}