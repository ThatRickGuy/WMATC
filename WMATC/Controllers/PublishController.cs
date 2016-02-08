using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMATC.Models;

namespace WMATC.Controllers
{
    public class PublishController : Controller
    {
        // GET: Publish
        public ActionResult Index()
        {
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            int SelectedEventId;
            int.TryParse(Session["SelectedEventId"].ToString (), out SelectedEventId);

            var PublishView = new ViewModels.PublishViewModel(SelectedEventId );
            return View(PublishView);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
    }
}
