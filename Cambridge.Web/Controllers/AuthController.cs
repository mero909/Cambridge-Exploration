using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Cambridge.Data.Models;
using Cambridge.Web.Models;

namespace Cambridge.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly CambridgeContext _context;

        public AuthController(CambridgeContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Login(LoginViewModel model)
        {
            if (String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Passcode)) return RedirectToAction("Login");

            var result = _context.Database.SqlQuery<Contact>("dbo.Contact_Authenticate @Email, @Password",
                new SqlParameter("Email", model.Email),
                new SqlParameter("Password", model.Passcode)).FirstOrDefault();

            if (result == null) return RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Dashboard", new { contactId = result.ID });
        }

        public string GetFullName(int id)
        {
            var name = _context.Contacts.FirstOrDefault(x => x.ID == id);
            return name == null ? String.Empty : name.Name;
        }
	}
}