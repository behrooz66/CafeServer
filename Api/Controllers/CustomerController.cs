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
        private IRestaurantRepository _restaurant;
        private ICityRepository _city;
        private IHelper _helper;
        public CustomerController(ICustomerRepository rep, 
                                  IAuthRepository auth,
                                  IRestaurantRepository restaurant,
                                  ICityRepository city,
                                  IHelper helper)
        {
            this._auth = auth;
            this._helper = helper;
            this._rep = rep;
            this._restaurant = restaurant;
            this._city = city;
        }

        
        [HttpGet]
        [Route("get/{id}")]
        [Authorize]
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
        [Authorize]
        public ActionResult GetByRestaurant()
        {
            var id = this._helper.GetUserEntity(User, this._auth).RestaurantId;
            var roles = this.User.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();
            IEnumerable<Customer> cus;
            if (roles.Contains("Manager"))
                cus = this._rep.GetByRestaurant(id, true);
            else
                cus = this._rep.GetByRestaurant(id, false);
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
            customer.UpdatedAt = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(_helper.GetTimeZone(User, _auth, _restaurant, _city)));
            customer.UpdatedBy = this._helper.GetUsername(User);
            this._rep.Create(customer);
            return Ok(customer.Id);
        }

        [HttpPut]
        [Route("put/{id}")]
        [Authorize]
        public ActionResult Put(int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(ModelState.ToList()));

            // protecting against manipulating other restaurants' data
            if (this._rep.Get(id).RestaurantId != this._helper.GetUserEntity(User, this._auth).RestaurantId)
                return Forbid(new string[] { "kir!" });

            customer.UpdatedAt = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(_helper.GetTimeZone(User, _auth, _restaurant, _city)));
            customer.UpdatedBy = this._helper.GetUsername(User);
            var c = this._rep.Update(id, customer);
            return Ok(c);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles ="Manager")]
        public ActionResult Delete(int id)
        {
            try
            {
                // protecting against deleting other restaurants data
                if (this._rep.Get(id).RestaurantId != this._helper.GetUserEntity(User, this._auth).RestaurantId)
                    return Forbid();

                bool result = this._rep.Delete(id);
                if (result) return Ok(id);
                return BadRequest("Error in deleting");
            }
            catch (NullReferenceException)
            {
                return NotFound("Record not found.");
            }
        }

        [HttpPut]
        [Route("archive/{id}")]
        [Authorize]
        public ActionResult Archive(int id)
        {
            if (this._rep.Get(id).RestaurantId != this._helper.GetUserEntity(User, this._auth).RestaurantId)
                return Forbid();

            bool result = this._rep.Archive(id);
            if (result)
                return Ok(id);
            return BadRequest("Error in deleting.");
        }

        [HttpGet]
        [Route("history/{id}")]
        [Authorize]
        public ActionResult History(int id)
        {
            if (!this._helper.OwnesCustomer(User, id, this._rep, this._auth))
                return Forbid();
            var histories = this._rep.GetHistory(id);
            return Ok(histories);
        }


        // SUMMARIES
        [HttpGet]
        [Route("orderSummary/{id}")]
        [Authorize]
        public ActionResult OrderSummary(int id)
        {
            if (!this._helper.OwnesCustomer(User, id, this._rep, this._auth))
                return Forbid();
            var summary = this._rep.OrderSummary(id);
            return Ok(summary);
        }

        [HttpGet]
        [Route("giftCardSummary/{id}")]
        [Authorize]
        public ActionResult GiftCardSummary(int id)
        {
            if (!this._helper.OwnesCustomer(User, id, this._rep, this._auth))
                return Forbid();
            var summary = this._rep.GiftCardSummary(id);
            return Ok(summary);
        }

        [HttpGet]
        [Route("reservationSummary/{id}")]
        [Authorize]
        public ActionResult ReservationSummary(int id)
        {
            if (!this._helper.OwnesCustomer(User, id, this._rep, this._auth))
                return Forbid();
            var summary = this._rep.ReservationSummary(id);
            return Ok(summary);
        }
    }
}
