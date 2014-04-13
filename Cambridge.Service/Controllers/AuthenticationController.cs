using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using Cambridge.Data.Models;
using Newtonsoft.Json;

namespace Cambridge.Service.Controllers
{
    public class AuthenticationController : ApiController
    {
        private readonly CambridgeContext _context;

        public AuthenticationController(CambridgeContext context)
        {
            _context = context;
        }

        // GET api/authentication
        public int Get(string email, string password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password)) return 0;

            var result = _context.Database.SqlQuery<Contact>("dbo.Contact_Authenticate @Email, @Password",
                new SqlParameter("Email", email),
                new SqlParameter("Password", password)).FirstOrDefault();

            return result == null ? 0 : Convert.ToInt32(result);
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