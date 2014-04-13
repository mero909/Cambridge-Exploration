using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Cambridge.Data.Models;
using Cambridge.Web.Models;
using PdfSharp.Pdf.IO;

namespace Cambridge.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly CambridgeContext _context;

        public RegisterController(CambridgeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// /Home
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var randomVal = new Random();
            
            var term1 = randomVal.Next(0, 10);
            var term2 = randomVal.Next(0, 10);
            Session["solution"] = term1 + term2;

            var register = new RegisterViewModel
            {
                States = _context.States.ToList(),
                SelectedState = new State(),
                InvestorTypes = _context.InvestorTypes.ToList(),
                Solution = Convert.ToInt32(Session["solution"]),
                Term1 = term1,
                Term2 = term2
            };

            return View(register);
        }

        /// <summary>
        /// Submits the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(RegisterViewModel model)
        {
            if (model.Solution != Convert.ToInt32(Session["solution"])) return new RedirectResult(Url.Action("Index", "Register"));

            // Redirect to login page if email already exists
            var exists = _context.Contacts.FirstOrDefault(x => x.Email == model.Email);
            if (exists != null) return Json(0, JsonRequestBehavior.AllowGet);

            // If email does not exist, continue on to saving the contact.
            try
            {
                var customFileName = CustomFileName(model.Email);

                var result = _context.Database.SqlQuery<Int32>(
                    "dbo.Contact_Save @Name, @Email, @Passcode, @Phone" +
                    ", @Address, @Address2, @City, @StateID, @PostalCode" +
                    ", @IsLocked, @IsDeleted, @InvestorType_1, @InvestorType_2" +
                    ", @InvestorType_3, @InvestorType_4, @CustomFileName",
                    new SqlParameter("Name", model.Name),
                    new SqlParameter("Email", model.Email),
                    new SqlParameter("Passcode", ConfigurationManager.AppSettings["MasterPasscode"]),
                    new SqlParameter("Phone", model.Phone),
                    new SqlParameter("Address", model.Address),
                    new SqlParameter("Address2", model.Address2 ?? String.Empty),
                    new SqlParameter("City", model.City),
                    new SqlParameter("StateID", model.StateId),
                    new SqlParameter("PostalCode", model.PostalCode),
                    new SqlParameter("IsLocked", model.IsLocked),
                    new SqlParameter("IsDeleted", model.IsDeleted),
                    new SqlParameter("InvestorType_1", Convert.ToBoolean(model.InvestorType1) ? 1 : 0),
                    new SqlParameter("InvestorType_2", Convert.ToBoolean(model.InvestorType2) ? 2 : 0),
                    new SqlParameter("InvestorType_3", Convert.ToBoolean(model.InvestorType3) ? 3 : 0),
                    new SqlParameter("InvestorType_4", Convert.ToBoolean(model.InvestorType4) ? 4 : 0),
                    new SqlParameter("CustomFileName", customFileName)).FirstOrDefault();

                if (result == 0) return new RedirectResult(Url.Action("Index"));

                // Create the new PDF and set Permissions.
                SetSecurityToPdf(model, customFileName);

                // send business email
                using (var msg = new MailMessage())
                {
                    msg.To.Add(new MailAddress("dennisandchristie@gmail.com"));
                    msg.Bcc.Add(new MailAddress("steven.rogers909@gmail.com"));
                    msg.From = new MailAddress("noreply@cambridgeexploration.com");
                    msg.Subject = "CambridgeExploration.com - New Registrant";
                    msg.Body = BuildMessage(model);
                    msg.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        smtp.Send(msg);
                    }
                }

                // send client email
                using (var msg = new MailMessage())
                {
                    msg.To.Add(new MailAddress(model.Email.Trim()));
                    msg.From = new MailAddress("noreply@cambridgeexploration.com");
                    msg.Subject = "Cambridge Exploration - Thank you for your interest!";
                    msg.Body = BuildContactMessage(model);
                    msg.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        smtp.Send(msg);
                    }
                }

                // Create security cookie
                var logInCookie = new HttpCookie("Email")
                {
                    Value = model.Email.Trim(),
                    Expires = DateTime.Now.AddHours(2)
                };
                HttpContext.Response.Cookies.Add(logInCookie);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Customs the name of the file.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        private String CustomFileName(String email)
        {
            return String.Concat(email.Replace("@", "-at-").Replace(" ", "").Replace(".", "-dot-").Trim(), "-cambridge-prospectise.pdf");
        }

        /// <summary>
        /// Sets the security to PDF.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="customFileName">Name of the custom file.</param>
        private void SetSecurityToPdf(RegisterViewModel model, String customFileName)
        {
            var newFile = Server.MapPath(String.Concat("~/PDFs/", customFileName));
            var checkFile = new FileInfo(newFile);

            if (!checkFile.Exists)
            {
                var file = new FileInfo(Server.MapPath("~/Content/Original_Prospectise.pdf"));
                file.CopyTo(newFile);
            }

            // Open an existing document. Providing an unrequired password is ignored.
            var document = PdfReader.Open(newFile);

            var securitySettings = document.SecuritySettings;

            // Setting one of the passwords automatically sets the security level to 
            // PdfDocumentSecurityLevel.Encrypted128Bit.
            securitySettings.UserPassword = model.Passcode;
            securitySettings.OwnerPassword = ConfigurationManager.AppSettings["MasterPasscode"];

            // Don't use 40 bit encryption unless needed for compatibility reasons
            //securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted40Bit;

            // Restrict some rights.
            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = true;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = true;
            securitySettings.PermitPrint = false;

            // Save the document...
            document.Save(newFile);
        }

        public ActionResult RegeneratePdfs(string passcode)
        {
            if (String.IsNullOrEmpty(passcode) && passcode != "ce19782014")
                return Content("<b>Please supply the passcode in the url.</b>");

            

            var users = _context.Contacts.ToList();

            if (!users.Any()) return Content("No Users Present or there was a database problem.");

            var sp = new StringBuilder();

            foreach (var user in users)
            {
                var viewModel = new RegisterViewModel { Email = user.Email, Passcode = ConfigurationManager.AppSettings["MasterPasscode"] };
                var fileName = CustomFileName(user.Email);

                sp.AppendLine("<span style=\"color: #000;\">Contact: " + user.Name + " - [" + user.Email + "]</span>");

                try
                {
                    SetSecurityToPdf(viewModel, fileName);
                    sp.AppendLine("<span style=\"color: green\">PDF Regenerated successfully.</span><br />");

                    // send client email
                    using (var msg = new MailMessage())
                    {
                        msg.To.Add(new MailAddress(viewModel.Email.Trim()));
                        msg.From = new MailAddress("noreply@cambridgeexploration.com");
                        msg.Subject = "Cambridge Exploration - A New Prospectus!";
                        msg.Body = BuildRegenMessage();
                        msg.IsBodyHtml = true;

                        using (var smtp = new SmtpClient())
                        {
                            smtp.Send(msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    sp.AppendLine("<span style=\"color: #f00; font-size: 14px;\">PDF Regenerated Failed.</span><br />Reason: " + ex.Message +
                                  "<br />StackTrace: " + ex.StackTrace + "<br />");
                }
            }

            return Content(sp.ToString());
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private String BuildMessage(RegisterViewModel model)
        {
            var state = _context.States.FirstOrDefault(x => x.ID == model.StateId);
            var contacts = _context.Contacts.FirstOrDefault(x => x.ID == model.Id);

            // Build Message Body
            var sb = new StringBuilder();
            sb.AppendFormat("Name: {0}<br />", model.Name);
            sb.AppendFormat("Email: {0}<br />", model.Email);
            sb.AppendFormat("Phone: {0}<br />", model.Phone);
            sb.AppendFormat("Address: {0}<br />", model.Address);

            if (!String.IsNullOrEmpty(model.Address2))
                sb.AppendFormat("Apt/Suite: {0}<br />", model.Address2);

            sb.AppendFormat("City: {0}<br />", model.City);
            sb.AppendFormat("State: {0}<br />", (state == null) ? "n/a" : state.Name);
            sb.AppendFormat("Zipcode: {0}<br />", model.PostalCode);
            sb.AppendFormat("City: {0}<br />", model.City);

            if (contacts != null && contacts.InvestorTypes.Any())
            {
                sb.Append("Investor Requirements:<br /><br />");
                sb.Append("<ul>");

                foreach (var investorType in contacts.InvestorTypes)
                {
                    sb.Append("<li>").Append(investorType.Name).Append("</li><br />");
                }

                sb.Append("</ul><br /><br />");
            }

            sb.AppendFormat("Message: {0}<br />", model.Message);

            return sb.ToString();
        }

        private String BuildContactMessage(RegisterViewModel model)
        {
            // Build Message Body
            var sb = new StringBuilder();

            sb.AppendLine("<h1>Thank you!</h1>");
            sb.AppendLine("Thank you, " + model.Name + "<br />");
            sb.AppendLine("Your passcode to unlock your prospectus is: " +
                          ConfigurationManager.AppSettings["MasterPasscode"] + "<br />");

            return sb.ToString();
        }

        private String BuildRegenMessage()
        {
            // Build Message Body
            var sb = new StringBuilder();

            sb.AppendLine("<h1>A New Prospectus!</h1>");
            sb.AppendLine("A new prospectus is available on Cambridge Exploration's web site!<br />");
            sb.AppendLine("Your passcode to unlock your prospectus is: <b>" +
                          ConfigurationManager.AppSettings["MasterPasscode"] + "</b><br />");

            return sb.ToString();
        }
	}
}