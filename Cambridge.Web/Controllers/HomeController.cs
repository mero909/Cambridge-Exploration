using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Cambridge.Data.Models;

namespace Cambridge.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly CambridgeContext _context;

        public HomeController(CambridgeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Perspectices this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Perspectice()
        {
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public ActionResult SendEmail(String email)
        {
            var contact = _context.Contacts.FirstOrDefault(x => x.Email == email.Trim());

            if (contact == null) return Content("The email entered was not found.");

            var url = GenerateNewUrl(contact.ID);
            var emailContent = BuildEmailMessage(contact.Name, url);

            using (var msg = new MailMessage())
            {
                msg.To.Add(new MailAddress(email.Trim()));
                msg.From = new MailAddress("noreply@cambridgeexploration.com");
                msg.Subject = "CambridgeExploration.com - Password Reset";
                msg.Body = emailContent;
                msg.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Send(msg);
                }
            }

            return Content("success");
        }

        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <returns></returns>
        public JsonResult ForgotPassword()
        {
            // dennisandchristie@gmail.com



            return Json("Forgot Password", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Builds the email message.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private String BuildEmailMessage(String name, String url)
        {
            String emailContent;
            using (var sr = new StreamReader(Server.MapPath("~/Content/PasswordReset.txt")))
            {
                emailContent = sr.ReadToEnd();
                emailContent = emailContent.Replace("[Name]", name).Replace("[Url]", url);
            }

            return emailContent;
        }

        /// <summary>
        /// Generates the new URL.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        private String GenerateNewUrl(Int32 contactId)
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");

            // save new guid
            var passwordExpirationUrl = new PasswordExpirationUrl
            {
                ContactID = contactId,
                IsExpired = false,
                Url = guid,
                CreateDate = DateTime.Now
            };

            _context.PasswordExpirationUrls.Add(passwordExpirationUrl);
            _context.SaveChanges();

            return String.Format(ConfigurationManager.AppSettings["PasswordResetUrl"], guid);
        }
    }
}