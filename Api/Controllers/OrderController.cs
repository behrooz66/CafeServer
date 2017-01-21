using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Helpers;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class OrderController: Controller
    {
        private IOrderRepository _rep;
        private IAuthRepository _auth;
        private IHelper _helper;
        private ICustomerRepository _customers;
        public OrderController(IOrderRepository rep, 
                               IAuthRepository auth,
                               IHelper helper,
                               ICustomerRepository customerRep)
        {
            this._helper = helper;
            this._auth = auth;
            this._rep = rep;
            this._customers = customerRep;
        }

        [HttpGet]
        [Route("get/{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                if (!this._helper.OwnesOrder(User, id, this._rep, this._customers, this._auth))
                    return Forbid();
                var o = this._rep.Get(id);
                return Ok(o);
            }
            catch(NullReferenceException)
            {
                return NotFound("The record could not be found.");
            }
        }

        [HttpGet]
        [Route("getByCustomer")]
        public ActionResult GetByCustomer(int customerId)
        {
            if (!this._helper.OwnesCustomer(User, customerId, this._customers, this._auth))
                return Forbid();
            var orders = this._rep.GetByCustomer(customerId).ToList();
            return Ok(orders);
        }

        [HttpGet]
        [Route("getByRestaurant")]
        public ActionResult GetByRestaurant()
        {
            int restaurantId = this._helper.GetUserEntity(User, this._auth).RestaurantId;
            var orders = this._rep.GetByRestaurant(restaurantId);
            return Ok(orders);
        }

        [HttpPost]
        [Route("post")]
        public ActionResult Post([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(this.ModelState.ToList()));

            if (!this._helper.OwnesCustomer(User, order.CustomerId, this._customers, this._auth))
                return Forbid();
            order.UpdatedAt = DateTime.Now;
            order.UpdatedBy = this._helper.GetUserId(User);
            this._rep.Create(order);
            return Ok(order.Id);
        }

        [HttpPut]
        [Route("put/{id}")]
        public ActionResult Put(int id, [FromBody] Order order )
        {
            if (id != order.Id)
                return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(this.ModelState.ToList()));

            if (!this._helper.OwnesOrder(User, id, this._rep, this._customers, this._auth))
                return Forbid();

            Order o = this._rep.Update(id, order);
            return Ok(o);
        }

        [HttpPut]
        [Route("archive/{id}")]
        public ActionResult Archive(int id)
        {
            if (!this._helper.OwnesOrder(User, id, this._rep, this._customers, this._auth))
                return Forbid();
            this._rep.Archive(id);
            return Ok(id + " archived.");
        }





    }
}
