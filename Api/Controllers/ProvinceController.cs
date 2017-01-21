using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthServer.RepositoryInterfaces;
using AuthServer.Repositories;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class ProvinceController: Controller
    {
        IProvinceRepository rep;
        public ProvinceController(IProvinceRepository _rep)
        {
            this.rep = _rep;
        }

        [HttpGet]
        [Route("getByCountry")]
        public ActionResult GetByCountry(int countryId)
        {
            var p = rep.GetByCountry(countryId);
            return Ok(p);
        }
    }
}
