using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Cambridge.Data.Models;

namespace Cambridge.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly CambridgeContext _context;

        public LoginController(CambridgeContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Logins the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public ActionResult Login(string email, string password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password)) return RedirectToAction("Login");

            var result = _context.Database.SqlQuery<Contact>("dbo.Contact_Authenticate @Email, @Password",
                new SqlParameter("Email", email),
                new SqlParameter("Password", password)).FirstOrDefault();

            return Json(result == null ? 0 : result.ID, JsonRequestBehavior.AllowGet);
        }

        public string GetFullName(int id)
        {
            var name = _context.Contacts.FirstOrDefault(x => x.ID == id);
            return name == null ? String.Empty : name.Name;
        }
	}
}