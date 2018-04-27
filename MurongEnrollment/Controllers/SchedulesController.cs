using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    [Authorize]
    public class SchedulesController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Index()
        {
            return View();

        }


        public ActionResult AddEditSchedulePartial(string ScheduleId)
        {
            ViewBag.subjects = unitOfWork.SubjectRepo.Fetch().Select(x => new { Name = x.SubjectCode + "-" + x.SubjectDescription, Id = x.Id }).ToList();
            ViewBag.gradelevel = unitOfWork.GradelevelRepo.Get();
            ViewBag.teacher = unitOfWork.TeacherRepo.Get();
            ViewBag.schoolyear = unitOfWork.SchoolYearRepo.Get();
            var model = unitOfWork.ScheduleRepo.Get(filter: m => m.Id == ScheduleId, includeProperties: "SchoolYears,Sections,Sections.GradeLevels,Subjects,Teachers").FirstOrDefault();
            return PartialView("_AddEditSchedulePartial", model);
        }
        public ActionResult SectionPartial(string GradeLevelId, string SectionId)
        {
            var model = unitOfWork.SectionRepo.Get(m => m.GradeLevelId == GradeLevelId);
            ViewBag.section = unitOfWork.SectionRepo.Find(m => m.Id == SectionId);
            return PartialView("_SectionPartial", model);
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var model = unitOfWork.ScheduleRepo.Get(includeProperties: "SchoolYears,Sections,Sections.GradeLevels,Subjects,Teachers");
            ViewBag.subjects = unitOfWork.SubjectRepo.Fetch().Select(x => new { Name = x.SubjectCode, Id = x.Id }).ToList();
            return PartialView("_GridViewPartial", model);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Schedules item)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    item.Id = Guid.NewGuid().ToString();
                    item.Days = string.Join(",", item.Day);
                    unitOfWork.ScheduleRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.ScheduleRepo.Get(includeProperties: "SchoolYears,Sections,Sections.GradeLevels,Subjects,Teachers");
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Schedules item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    item.Days = string.Join(",", item.Day);
                    unitOfWork.ScheduleRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.ScheduleRepo.Get(includeProperties: "SchoolYears,Sections,Sections.GradeLevels,Subjects,Teachers");
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Schedules item)
        {

            if (item != null)
            {
                try
                {
                    unitOfWork.ScheduleRepo.Delete(unitOfWork.ScheduleRepo.Find(m => m.Id == item.Id));
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.ScheduleRepo.Get(includeProperties: "SchoolYears,Sections,Sections.GradeLevels,Subjects,Teachers");
            return PartialView("_GridViewPartial", model);
        }

        public ActionResult SchedulePrintPreviewPartial(string SchoolYearId)
        {
            var model = unitOfWork.ScheduleRepo.Get(m => m.SchoolYearId == SchoolYearId);
            ScheduleReport report = new ScheduleReport() { DataSource = model };
            return PartialView("_SchedulePrintPreviewPartial", report);
        }
    }
}