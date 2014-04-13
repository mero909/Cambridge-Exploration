using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cambridge.Data.Models;

namespace Cambridge.Web.Controllers
{
    public class PasswordResetController : Controller
    {
        private readonly CambridgeContext _context;
        //private Contact contact;

        public PasswordResetController(CambridgeContext context)
        {
            _context = context;
        }

        //
        // GET: /PasswordReset/
        //public ActionResult Index(string k)
        //{
        //    var guid = _context.PasswordExpirationUrls.FirstOrDefault(x => x.Url.ToLower() == k.ToLower());

        //    if (guid == null) return View();

        //    return View();
        //}

        //public ActionResult ResetPassword(String password)
        //{
        //    var contact = 
        //}
	}
}