using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using AuthServer.Models;
using System.Security.Claims;
using Api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController: Controller
    {
        private ICustomerRepository _rep;
        private IAuthRepository _auth;
        private IHelper _helper;
        public CustomerController(ICustomerRepository rep, 
                                  IAuthRepository auth,
                                  IHelper helper)
        {
            this._auth = auth;
            this._helper = helper;
            this._rep = rep;
        }

        [Authorize]
        [HttpGet]
        [Route("get/{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var customer = this._rep.Get(id);
                if (this._helper.GetUserEntity(User, this._auth).RestaurantId == customer.RestaurantId)
                    return Ok(customer);
                else
                    return Forbid();
            }
            catch (NullReferenceException)
            {
                return NotFound("Record not found");
            }
        }

        [HttpGet]
        [Route("get")]
        public ActionResult GetByRestaurant()
        {
            var id = this._helper.GetUserEntity(User, this._auth).RestaurantId;
            var cus = this._rep.GetByRestaurant(id);
            return Ok(cus);
        }

        [HttpPost]
        [Authorize]
        [Route("post")]
        public ActionResult Post([FromBody] Customer customer)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(ModelState.ToList()));

            customer.RestaurantId = this._helper.GetUserEntity(User, this._auth).RestaurantId;
            customer.UpdatedAt = DateTime.Now;
            customer.UpdatedBy = this._helper.GetUsername(User);
            this._rep.Create(customer);
            return Ok(customer.Id);
        }

        [HttpPut]
        [Route("put/{id}")]
        public ActionResult Put(int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(ModelState.ToList()));

            // protecting against manipulating other restaurants' data
            if (this._rep.Get(id).RestaurantId != this._helper.GetUserEntity(User, this._auth).RestaurantId)
                return Forbid(new string[] { "kir!" });

            customer.UpdatedAt = DateTime.Now;
            customer.UpdatedBy = this._helper.GetUsername(User);
            var c = this._rep.Update(id, customer);
            return Ok(c);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                // protecting against deleting other restaurants data
                if (this._rep.Get(id).RestaurantId != this._helper.GetUserEntity(User, this._auth).RestaurantId)
                    return Forbid();

                bool result = this._rep.Delete(id);
                if (result) return Ok(id + " deleteed");
                return BadRequest("Error in deleting");
            }
            catch (NullReferenceException)
            {
                return NotFound("Record not found.");
            }
        }
    }
}
