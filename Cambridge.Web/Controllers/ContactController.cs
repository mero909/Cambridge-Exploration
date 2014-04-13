using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Cambridge.Data.Models;
using Cambridge.Web.Models;

namespace Cambridge.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly CambridgeContext _context;

        public ContactController(CambridgeContext context)
        {
            _context = context;
        }

        //
        // GET: /Contact/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(ContactViewModel model)
        {
            try
            {
                using (var msg = new MailMessage())
                {
                    msg.To.Add(new MailAddress("dennisandchristie@gmail.com"));
                    msg.Bcc.Add(new MailAddress("steven.rogers909@gmail.com"));
                    msg.From = new MailAddress("noreply@cambridgeexploration.com");
                    msg.Subject = "CambridgeExploration.com - Contact Us";
                    msg.Body = BuildMessage(model);
                    msg.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        smtp.Send(msg);
                    }
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private String BuildMessage(ContactViewModel model)
        {
            // Build Message Body
            var sb = new StringBuilder();

            sb.Append("<h2>Contact Us Form Submission</h2>");

            sb.AppendLine("<p>This email is generated from the 'Contact Us' page.</p>");

            sb.AppendFormat("Name: {0}<br />", model.Name);
            sb.AppendFormat("Email: {0}<br />", model.Email);
            sb.AppendFormat("Phone: {0}<br />", model.Phone);
            sb.AppendFormat("Best Time to Call: {0}<br /><br />", model.TimeToCall);
            sb.AppendFormat("Message: {0}<br />", model.Message);

            return sb.ToString();
        }
	}
}