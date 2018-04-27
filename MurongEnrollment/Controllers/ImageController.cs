using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MurongEnrollment.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult PhotoUploadControlUpload()
        {
            var res = BinaryImageEditExtension.GetCallbackResult();

            return res;
        }
    }
}