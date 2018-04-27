using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    public class AlumniController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Alumni
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult AlumniGridViewPartial()
        {
            var model = unitOfWork.AlumniRepo.Get(includeProperties: "Enrollments.Students,Enrollments.SchoolYears");
            return PartialView("_AlumniGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AlumniGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Alumni item)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    var EnrollmentId = Request.Params["Id"];
                    unitOfWork.AlumniRepo.Insert(new Models.Alumni() { Id = Guid.NewGuid().ToString(), EnrollmentId = EnrollmentId });
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.AlumniRepo.Get(includeProperties: "Enrollments.Students,Enrollments.SchoolYears");
            return PartialView("_AlumniGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AlumniGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Alumni item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_AlumniGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AlumniGridViewPartialDelete(System.String Id)
        {
            //var model = new object[0];
            if (Id != null)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.AlumniRepo.Delete(unitOfWork.AlumniRepo.Find(m => m.Id == Id));
                    unitOfWork.Save();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.AlumniRepo.Get(includeProperties: "Enrollments.Students,Enrollments.SchoolYears");
            return PartialView("_AlumniGridViewPartial", model);
        }
        public ActionResult AddEditAlumniStudentPartial(string EnrollmentId)
        {
            var model = unitOfWork.EnrollmentsRepo.Find(m => m.Id == EnrollmentId, includeProperties: "Students");
            return PartialView("_AddEditAlumniStudentPartial", model);
        }
        //public ActionResult AddEditAlumniStudentPartialcallBack(string StudentId)
        //{
        //    ViewBag.StudentId = StudentId;
        //    var model = unitOfWork.StudentRepo.Find(m => m.Id== StudentId, includeProperties: "");
        //    return PartialView("_AddEditAlumniStudentPartial", model);
        //}
    }
}