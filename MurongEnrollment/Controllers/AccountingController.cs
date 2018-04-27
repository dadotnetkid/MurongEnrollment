using DevExpress.Web.Mvc;
using MurongEnrollment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    [Authorize]
    public class AccountingController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Index()
        {
            return View();
        }
        #region Students
        [ValidateInput(false)]
        public ActionResult StudentGridViewPartial()
        {
            var model = unitOfWork.StudentRepo.Get(includeProperties: "Enrollments");
            return PartialView("_StudentGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StudentGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Students item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_StudentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult StudentGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Students item)
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
            return PartialView("_StudentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult StudentGridViewPartialDelete(System.String Id)
        {
            var model = new object[0];
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
            return PartialView("_StudentGridViewPartial", model);
        }
        public ActionResult AddEditStudentPartial(string StudentId)
        {
            var model = unitOfWork.StudentRepo.Find(m => m.Id == StudentId);
            return PartialView("_AddEditStudentPartial");
        }
        #endregion

        #region Payment
        [ValidateInput(false)]
        public ActionResult PaymentGridViewPartial(string EnrollmentId)
        {
            //   var EnrollmentId = Request.Params["EnrollmentId"];
            ViewBag.EnrollmentId = EnrollmentId;
            //var model = unitOfWork..Get(filter: m=>,includeProperties:"");
            var model = unitOfWork.PaymentsRepo.Get(m => m.EnrollmentId == EnrollmentId, includeProperties: "Enrollments,Enrollments.SchoolYears");
            return PartialView("_PaymentGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Payments item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (item.PaymentAmount > unitOfWork.EnrollmentsRepo.Find(x => x.Id == item.EnrollmentId).TotalBalance)
                    {
                        ViewData["EditError"] = "Payment should not exceed the total balance";
                    }
                    else
                    {
                        item.Id = Guid.NewGuid().ToString();
                        unitOfWork.PaymentsRepo.Insert(item);
                        unitOfWork.Save();
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.PaymentsRepo.Get(m => m.EnrollmentId == item.EnrollmentId, includeProperties: "Enrollments,Enrollments.SchoolYears");
            return PartialView("_PaymentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Payments item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    //if (item.PaymentAmount > unitOfWork.EnrollmentsRepo.Find(x => x.Id == item.EnrollmentId).TotalBalance)
                    //{
                    //    ViewData["EditError"] = "Payment should not exceed the total balance";
                    //}
                    //else
                    //{
                    //    unitOfWork.PaymentsRepo.Update(item);
                    //    unitOfWork.Save();
                    //}
                    unitOfWork.PaymentsRepo.Update(item);
                    unitOfWork.Save();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.PaymentsRepo.Get(m => m.EnrollmentId == item.EnrollmentId, includeProperties: "Enrollments,Enrollments.SchoolYears");
            return PartialView("_PaymentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Payments item)
        {
            
            if (item != null)
            {
                try
                {
                    unitOfWork.PaymentsRepo.Delete(unitOfWork.PaymentsRepo.Find(m => m.Id == item.Id));
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.PaymentsRepo.Get(m => m.EnrollmentId == item.EnrollmentId, includeProperties: "Enrollments,Enrollments.SchoolYears");
            return PartialView("_PaymentGridViewPartial", model);
        }

        #endregion

        #region Standard Fees
        [ValidateInput(false)]
        public ActionResult StandardFeeGridViewPartial(string EnrollmentId)
        {
            ViewBag.EnrollmentId = EnrollmentId;
            var enrollments = unitOfWork.EnrollmentsRepo.Find(filter: m => m.Id == EnrollmentId);
            var model = unitOfWork.StandardFeeRepo.Get(m => m.GradeLevelId == enrollments.Sections.GradeLevelId);
            //unitOfWork.StandardFeeRepo.Get(m => m.GradeLevels.Sections.Any(x => x.Enrollments.Any(y => y.StudentId == StudentId)));
            return PartialView("_StandardFeeGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StandardFeeGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Enrollments item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_StandardFeeGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult StandardFeeGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Enrollments item)
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
            return PartialView("_StandardFeeGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult StandardFeeGridViewPartialDelete(System.String Id)
        {
            var model = new object[0];
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
            return PartialView("_StandardFeeGridViewPartial", model);
        }
        #endregion

        #region Enrollment

        [ValidateInput(false)]
        public ActionResult EnrollmentGridViewPartial(string StudentId)
        {
            ViewBag.StudentId = StudentId;
            var model = unitOfWork.EnrollmentsRepo.Get(filter: m => m.StudentId == StudentId, includeProperties: "Students,SchoolYears,Sections.GradeLevels");
            return PartialView("_EnrollmentGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EnrollmentGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Enrollments item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_EnrollmentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EnrollmentGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] MurongEnrollment.Models.Enrollments item)
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
            return PartialView("_EnrollmentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EnrollmentGridViewPartialDelete(System.String Id)
        {
            var model = new object[0];
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
            return PartialView("_EnrollmentGridViewPartial", model);
        }
        #endregion
        #region Reports
        public ActionResult PrintReceiptPartial(string PaymentId)
        {
            var model = unitOfWork.PaymentsRepo.Get(m => m.Id == PaymentId, includeProperties: "Enrollments.Students");
            ReceiptReport report = new ReceiptReport() { DataSource = model };
            
            return PartialView("_PrintReceiptPartial", report);
        }
        #endregion

    }
}