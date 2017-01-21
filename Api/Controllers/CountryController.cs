using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class CountryController : Controller
    {
        private ICountryRepository _rep;
        public CountryController(ICountryRepository rep)
        {
            this._rep = rep;
        }

        [HttpGet]
        [Route("get")]
        [Authorize]
        public ActionResult Get() {
            var c = this._rep.GetAll();
            return Ok(c);
        }

        [HttpGet]
        [Route("get/{id}")]
        [Authorize]
        public ActionResult Get(int id)
        {
            var c = this._rep.Get(id);
            return Ok(c);
        }
    }
}
