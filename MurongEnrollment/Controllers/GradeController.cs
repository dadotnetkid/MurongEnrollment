using DevExpress.Web.Mvc;
using MurongEnrollment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    public class GradeController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Grade
        public ActionResult Index()
        {
            return View();
        }




        [ValidateInput(false)]
        public ActionResult GradeGridViewPartial(string EnrollmentId = "", string GradingId = "")
        {
            //ViewBag.EnrolledSubjectId = EnrolledSubjectId;
            ViewBag.EnrollmentId = EnrollmentId;
            List<Grades> model = unitOfWork.GradesRepo.Get(m => m.EnrolledSubjects.EnrollmentId == EnrollmentId && m.GradingId == GradingId).ToList();
            //var subjects = model.Select(m => m.EnrolledSubjectId);

            return PartialView("_GradeGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GradeGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Grades item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    item.Id = Guid.NewGuid().ToString();
                    unitOfWork.GradesRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var EnrollmentId = Request.Params["EnrollmentId"];
            var GradingId = Request.Params["GradingId"];
            ViewBag.EnrollmentId = EnrollmentId;
            List<Grades> model = unitOfWork.GradesRepo.Get(m => m.EnrolledSubjects.EnrollmentId == EnrollmentId && m.GradingId == GradingId).ToList();

            return PartialView("_GradeGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GradeGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Grades item)
        {
            //var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.GradesRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var EnrollmentId = Request.Params["EnrollmentId"];
            var GradingId = Request.Params["GradingId"];
            ViewBag.EnrollmentId = EnrollmentId;
            List<Grades> model = unitOfWork.GradesRepo.Get(m => m.EnrolledSubjects.EnrollmentId == EnrollmentId && m.GradingId == GradingId).ToList();
            return PartialView("_GradeGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GradeGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Grades item)
        {

            if (item != null)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.GradesRepo.Delete(unitOfWork.GradesRepo.Find(m => m.Id == item.Id));
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var EnrollmentId = Request.Params["EnrollmentId"];
            var GradingId = Request.Params["GradingId"];
            ViewBag.EnrollmentId = EnrollmentId;
            List<Grades> model = unitOfWork.GradesRepo.Get(m => m.EnrolledSubjects.EnrollmentId == EnrollmentId && m.GradingId == GradingId).ToList();
            return PartialView("_GradeGridViewPartial", model);
        }


        public ActionResult CboEnrollmentPartial(string StudentId)
        {
            var model = unitOfWork.EnrollmentsRepo.Get(m => m.StudentId == StudentId);
            return PartialView("_CboEnrollmentPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult GradeReportPartial(string EnrollmentId = "", string GradingId = "")
        {
            //ViewBag.EnrolledSubjectId = EnrolledSubjectId;
            ViewBag.EnrollmentId = EnrollmentId;
            List<Grades> model = unitOfWork.GradesRepo.Get(m => m.EnrolledSubjects.EnrollmentId == EnrollmentId && m.GradingId == GradingId).ToList();
            //var subjects = model.Select(m => m.EnrolledSubjectId);
            GradeReport report = new GradeReport() { DataSource = model };
            return PartialView("_GradeReportPartial", report);
        }

        public ActionResult AddEditGradePartial(string EnrollmentId)
        {
            var grades = new UnitOfWork().GradesRepo.Get(m => m.EnrolledSubjects.EnrollmentId == EnrollmentId).Select(x => x.EnrolledSubjectId);
            ViewBag.Subjects = new UnitOfWork().EnrolledSubjectsRepo.Get(m => m.EnrollmentId == EnrollmentId && !grades.Contains(m.Id));
            return PartialView("_AddEditGradePartial");
        }
    }
}