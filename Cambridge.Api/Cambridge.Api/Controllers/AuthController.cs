using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Cambridge.Data.Models;

namespace Cambridge.Api.Controllers
{
    public class AuthController : ApiController
    {
        private readonly CambridgeContext _context;

        public AuthController(CambridgeContext context)
        {
            _context = context;
        }

        // GET api/auth
        public JsonResult Get(string email, string password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password)) return RedirectToAction("Login");

            var result = _context.Database.SqlQuery<Contact>("dbo.Contact_Authenticate @Email, @Password",
                new SqlParameter("Email", email),
                new SqlParameter("Password", password)).FirstOrDefault();

            return Json(result == null ? 0 : result.ID, JsonRequestBehavior.AllowGet);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}