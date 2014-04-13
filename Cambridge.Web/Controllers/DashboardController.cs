using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cambridge.Data.Models;
using Cambridge.Web.Models;

namespace Cambridge.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly CambridgeContext _context;

        public DashboardController(CambridgeContext context)
        {
            _context = context;
        }

        //
        // GET: /Dashboard/
        public ActionResult Index(Int32 contactId)
        {
            if (HttpContext.Request.Cookies["Email"] == null && contactId <= 0) 
                HttpContext.Response.Redirect(Url.Action("Index", "Home"));

            var model = new DashboardViewModel
            {
                PrespectiseUrls = _context.ContactFiles.Where(x => x.ContactID == contactId).ToList()
            };

            foreach (var m in model.PrespectiseUrls)
            {
                m.FileName = String.Format(ConfigurationManager.AppSettings["PDFPath"], m.FileName);
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            // Destroy the cookie
            var cookie = new HttpCookie("Email")
            {
                Expires = DateTime.Now.AddHours(-1)
            };
            HttpContext.Response.Cookies.Add(cookie);

            return new RedirectResult(Url.Action("Index", "Home"));
        }
	}
}