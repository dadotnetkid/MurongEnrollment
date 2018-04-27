using DevExpress.Web.Mvc;
using MurongEnrollment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    [RoutePrefix("Enrollment")]
    [Authorize]
    public class EnrollmentController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        [Route("{StudentId}")]
        public ActionResult Index(string StudentId)
        {
            ViewBag.StudentId = StudentId;
            var schoolyearEnrolled = unitOfWork.EnrollmentsRepo.Get(m => m.StudentId == StudentId).Select(x=>x.SchoolYearId);
            ViewBag.SchoolYear = unitOfWork.SchoolYearRepo.Get(x => !schoolyearEnrolled.Contains(x.Id));
            return View(unitOfWork.StudentRepo.Find(m => m.Id == StudentId));
        }

        [ValidateInput(false)]
        [Route("AvailableSubjectGridViewPartial")]
        public ActionResult AvailableSubjectGridViewPartial(string StudentId)
        {
            ViewBag.StudentId = StudentId;
            //var student = unitOfWork.StudentRepo.Find(m => m.Id == StudentId);
            //var section = student.Enrollments.Where(x => x.SchoolYears.isActive == true).FirstOrDefault(); 
            string SectionId = Request.Params["SectionId"];
            string SchoolYearId = Request.Params["SchoolYearId"];
            var model = unitOfWork.ScheduleRepo.Get(filter: m => m.SchoolYearId == SchoolYearId && m.SectionId == SectionId, includeProperties: "Subjects,Sections,Sections.GradeLevels,Teachers");
            return PartialView("_AvailableSubjectGridViewPartial", model);
        }
        [Route("SectionPartial")]
        public ActionResult SectionPartial()
        {
            string GradeLevelId = Request.Params["GradeLevelId"];
            var model = unitOfWork.SectionRepo.Get(m => m.GradeLevelId == GradeLevelId);
            return PartialView("_SectionPartial", model);
        }
        [Route("AddNewSubjectPartial")]
        public ActionResult AddNewSubjectPartial()
        {
            string SectionId = Request.Params["SectionId"];
            string SchoolYearId = Request.Params["SchoolYearId"];
            string[] AddedSubjects = Request.Params["AddedSubjects"]?.Split(',') ?? new string[] { "" };

            ViewBag.AddedSubjects = unitOfWork.ScheduleRepo.Get(m => AddedSubjects.Contains(m.Id), includeProperties: "Subjects");

            var model = unitOfWork.ScheduleRepo.Get(m => m.SchoolYearId == SchoolYearId && m.Id != SectionId, includeProperties: "Subjects");
            return PartialView("_AddNewSubjectPartial", model);
        }
        [HttpPost, Route("EnrollStudent")]
        public JsonResult EnrollStudent()
        {
            var SchoolYearId = Request.Params["SchoolYearId"];
            var SectionId = Request.Params["SectionId"];
            var AddedSubjects = Request.Params["AddedSubjects"];
            var StudentId = Request.Params["StudentId"];
            var ScheduleRepo = unitOfWork.ScheduleRepo.Get(filter: m => m.SchoolYearId == SchoolYearId && m.SectionId == SectionId, includeProperties: "Subjects,Sections,Sections.GradeLevels,Teachers");
            var enrollment = new Models.Enrollments()
            {

                Id = Guid.NewGuid().ToString(),
                StudentId = StudentId,
                SchoolYearId = SchoolYearId,
                SectionId = SectionId,

            };

            unitOfWork.EnrollmentsRepo.Insert(enrollment);
            List<EnrolledSubjects> enrolledSubjects = new List<EnrolledSubjects>();
            foreach (var i in ScheduleRepo)
            {
                enrolledSubjects.Add(new EnrolledSubjects()
                {
                    Id = Guid.NewGuid().ToString(),
                    EnrollmentId = enrollment.Id,
                    ScheduleId = i.Id,

                });
            }
            foreach (var i in AddedSubjects?.Split(','))
            {
                if (i == "")
                    break;
                enrolledSubjects.Add(new EnrolledSubjects()
                {
                    Id = Guid.NewGuid().ToString(),
                    EnrollmentId = enrollment.Id,
                    ScheduleId = i
                });
            }
            unitOfWork.EnrolledSubjectsRepo.InsertRange(enrolledSubjects);
            unitOfWork.Save();


            return Json(new { SchoolYearId = SchoolYearId, SectionId = SectionId, AddedSubjects = AddedSubjects }, JsonRequestBehavior.AllowGet);
        }
    }
}