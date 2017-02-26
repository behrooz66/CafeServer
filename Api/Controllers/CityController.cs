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
    public class CityController: Controller
    {
        private ICityRepository _rep;
        public CityController(ICityRepository rep)
        {
            this._rep = rep;
        }

        [HttpGet]
        [Route("getByProvince")]
        //[Authorize]
        public ActionResult GetByProvince(int provinceId)
        {
            var c = this._rep.GetByProvince(provinceId);
            return Ok(c);
        }
    }
}
