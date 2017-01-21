using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class RestaurantController: Controller
    {
        private IRestaurantRepository _rep;
        public RestaurantController(IRestaurantRepository rep)
        {
            this._rep = rep;
        }


        
    }
}
