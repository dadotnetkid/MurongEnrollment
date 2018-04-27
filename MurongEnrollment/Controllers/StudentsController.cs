using DevExpress.Web.Mvc;
using MurongEnrollment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Students
        #region Students
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult StudentGridViewPartial()
        {
            var model = unitOfWork.StudentRepo.Get(includeProperties: "Enrollments,Enrollments.Sections,Enrollments.Sections.GradeLevels");
            return PartialView("_StudentGridViewPartial", model);
        }
        public ActionResult AddEditStudentPartial(string _Id)
        {
            var model = unitOfWork.StudentRepo.Get(includeProperties: "Enrollments,Enrollments.Sections,Enrollments.Sections.GradeLevels", filter: m => m.Id == _Id).FirstOrDefault();
            return PartialView("_AddEditStudentPartial", model);
        }
        public ActionResult SectionPartial(string GradeLevelId, string SectionId = "")
        {

            ViewBag.GradeLevel = unitOfWork.SectionRepo.Get(filter: m => m.GradeLevelId == GradeLevelId);
            return PartialView("_SectionPartial", new UnitOfWork().SectionRepo.Find(m => m.Id == SectionId));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult StudentGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Students item, string SectionId)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    item.Id = Guid.NewGuid().ToString();
                    /* item.Enrollments.Add(new Models.Enrollments()
                     {

                     });*/
                    //item.Enrollments.Id = Guid.NewGuid().ToString();
                    //item.Enrollments.Sections = unitOfWork.SectionRepo.Find(m => m.Id == SectionId);
                    unitOfWork.StudentRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.StudentRepo.Get(includeProperties: "Enrollments,Enrollments.Sections,Enrollments.Sections.GradeLevels");
            return PartialView("_StudentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult StudentGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Students item, string SectionId)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    unitOfWork.StudentRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.StudentRepo.Get(includeProperties: "Enrollments,Enrollments.Sections,Enrollments.Sections.GradeLevels");
            return PartialView("_StudentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult StudentGridViewPartialDelete(System.String Id)
        {

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
            var model = unitOfWork.StudentRepo.Get(includeProperties: "Enrollments,Enrollments.Sections,Enrollments.Sections.GradeLevels");
            return PartialView("_StudentGridViewPartial", model);
        }
        public ActionResult WebCamPartial()
        {
            return PartialView("_webcampartial");
        }
        #endregion

        #region Enrollment
        [ValidateInput(false)]
        public ActionResult EnrollmentGridViewPartial(string StudentId)
        {
            ViewBag.StudentId = StudentId;
            var model = unitOfWork.EnrollmentsRepo.Get(filter: m => m.StudentId == StudentId, includeProperties: "Sections.GradeLevels,Sections,SchoolYears,Sections.GradeLevels.StandardFees");
            return PartialView("_EnrollmentGridViewPartial", model);
        }

        #endregion
        #region Schedule

        [ValidateInput(false)]
        public ActionResult ScheduleGridViewPartial(string EnrollmentId)
        {
            var model = unitOfWork.EnrolledSubjectsRepo.Get(filter: m => m.EnrollmentId == EnrollmentId,
                includeProperties: "Schedules,Schedules.Subjects,Schedules.Teachers");
            ViewBag.EnrollmentId = EnrollmentId;
            return PartialView("_ScheduleGridViewPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult ScheduleGridViewPartialAddNew(string EnrollmentId, [ModelBinder(typeof(DevExpressEditorsBinder))] EnrolledSubjects item)
        {
            item.Id = Guid.NewGuid().ToString();
            unitOfWork.EnrolledSubjectsRepo.Insert(item);
            unitOfWork.Save();
            var model = unitOfWork.EnrolledSubjectsRepo.Get(filter: m => m.EnrollmentId == EnrollmentId,
                includeProperties: "Schedules,Schedules.Subjects,Schedules.Teachers");
            ViewBag.EnrollmentId = EnrollmentId;
            return PartialView("_ScheduleGridViewPartial", model);
        }
        [ValidateInput(false)]
        public ActionResult ScheduleGridViewPartialDelete(string EnrollmentId, [ModelBinder(typeof(DevExpressEditorsBinder))] EnrolledSubjects item)
        {
            unitOfWork.EnrolledSubjectsRepo.Delete(unitOfWork.EnrolledSubjectsRepo.Find(m => m.Id == item.Id));
            unitOfWork.Save();
            var model = unitOfWork.EnrolledSubjectsRepo.Get(filter: m => m.EnrollmentId == EnrollmentId,
                includeProperties: "Schedules,Schedules.Subjects,Schedules.Teachers");
            ViewBag.EnrollmentId = EnrollmentId;
            return PartialView("_ScheduleGridViewPartial", model);
        }

        public ActionResult AddEditEnrolledSubjectPartial(string EnrollmentSubjectId)
        {
            var model = unitOfWork.EnrolledSubjectsRepo.Find(m => m.Id == EnrollmentSubjectId);
            return PartialView("_AddEditEnrolledSubjectPartial", model);
        }

        public ActionResult ScheduleSubjectPartial(string SectionId)
        {
            var model = unitOfWork.ScheduleRepo.Get(m => m.SectionId == SectionId).Select(x => new { Id = x.Id, Name = x.Subjects.SubjectCode + " - " + x.Subjects.SubjectDescription });
            return PartialView("_ScheduleSubjectPartial", model);
        }
        #endregion
        #region Grade

        public ActionResult AddGradePartial(string EnrollmentId, [ModelBinder(typeof(DevExpressEditorsBinder))]  EnrolledSubjects model)
        {
            ViewBag.EnrollmentId = EnrollmentId;
            // ViewBag.Subjects = unitOfWork.EnrolledSubjectsRepo.Get(filter: m => m.EnrollmentId == EnrollmentId,includeProperties: "Schedules.Subjects").Select(x => new { Id=x.Id,Name=x.Schedules.Subjects.SubjectCode});
            if (model != null)
            {
                try
                {
                    var subject = unitOfWork.EnrolledSubjectsRepo.Find(m => m.Id == model.Id);
                    subject.Grade = model.Grade;
                    unitOfWork.EnrolledSubjectsRepo.Update(subject);
                    unitOfWork.Save();
                }
                catch (Exception)
                {

                }

            }
            return PartialView("_AddGradePartial", unitOfWork.EnrolledSubjectsRepo.Find(m => m.EnrollmentId == EnrollmentId));
        }
        #endregion


        #region Report
        public ActionResult AccessmentReportPartial(string EnrollmentId)
        {
            var model = unitOfWork.EnrollmentsRepo.Get(m => m.Id == EnrollmentId, includeProperties: "SchoolYears,Students,Sections.GradeLevels.StandardFees,EnrolledSubjects.Schedules.Subjects,EnrolledSubjects.Schedules,EnrolledSubjects.Schedules.Teachers");
            AccessmentReport accessmentReport = new AccessmentReport() { DataSource = model };
            return PartialView("_AccessmentReportPartial", accessmentReport);
        }
        #endregion
    }
}