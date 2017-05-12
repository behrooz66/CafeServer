using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Helpers;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
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
        private IOrderTypeRepository _orderTypes;

        public OrderController(IOrderRepository rep, 
                               IAuthRepository auth,
                               IHelper helper,
                               ICustomerRepository customerRep,
                               IOrderTypeRepository orderTypes)
        {
            this._helper = helper;
            this._auth = auth;
            this._rep = rep;
            this._customers = customerRep;
            this._orderTypes = orderTypes;
        }

        [HttpGet]
        [Route("get/{id}")]
        [Authorize]
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
        [Authorize]
        public ActionResult GetByCustomer(int customerId)
        {
            if (!this._helper.OwnesCustomer(User, customerId, this._customers, this._auth))
                return Forbid();
            var roles = this.User.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();
            IEnumerable<Order> orders;
            if (roles.Contains("Manager"))
                orders = this._rep.GetByCustomer(customerId, true).ToList();
            else
                orders = this._rep.GetByCustomer(customerId, false).ToList();

            return Ok(orders);
        }

        [HttpGet]
        [Route("getByRestaurant")]
        [Authorize]
        public ActionResult GetByRestaurant()
        {
            int restaurantId = this._helper.GetUserEntity(User, this._auth).RestaurantId;
            var orders = this._rep.GetByRestaurant(restaurantId);
            return Ok(orders);
        }

        [HttpPost]
        [Route("post")]
        [Authorize]
        public ActionResult Post([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(this.ModelState.ToList()));

            if (!this._helper.OwnesCustomer(User, order.CustomerId, this._customers, this._auth))
                return Forbid();
            order.UpdatedAt = DateTime.Now;
            order.UpdatedBy = this._helper.GetUsername(User);
            this._rep.Create(order);
            return Ok(order.Id);
        }

        [HttpPut]
        [Route("put/{id}")]
        [Authorize]
        public ActionResult Put(int id, [FromBody] Order order )
        {
            if (id != order.Id)
                return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(this.ModelState.ToList()));

            if (!this._helper.OwnesOrder(User, id, this._rep, this._customers, this._auth))
                return Forbid();

            order.UpdatedAt = DateTime.Now;
            order.UpdatedBy = this._helper.GetUsername(User);
            Order o = this._rep.Update(id, order);
            return Ok(o);
        }

        [HttpPut]
        [Route("archive/{id}")]
        [Authorize]
        public ActionResult Archive(int id)
        {
            if (!this._helper.OwnesOrder(User, id, this._rep, this._customers, this._auth))
                return Forbid();
            this._rep.Archive(id);
            return Ok(id);
        }

        [HttpPut]
        [Route("unarchive/{id}")]
        [Authorize]
        public ActionResult Unarchive(int id)
        {
            if (!this._helper.OwnesOrder(User, id, this._rep, this._customers, this._auth))
                return Forbid();
            this._rep.Unarchive(id);
            return Ok(id);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            if (!this._helper.OwnesOrder(User, id, this._rep, this._customers, this._auth))
                return Forbid();
            this._rep.Delete(id);
            return Ok(id);
        }

        [HttpGet]
        [Route("history/{id}")]
        [Authorize(Roles ="Manager")]
        public ActionResult History(int id)
        {
            if (!this._helper.OwnesOrder(User, id, this._rep, this._customers, this._auth))
                return Forbid();
            var histories = this._rep.GetHistory(id);
            return Ok(histories);
        }

        [HttpGet]
        [Route("types")]
        public ActionResult GetTypes()
        {
            var types = this._orderTypes.GetAll();
            return Ok(types);
        }

    }
}
