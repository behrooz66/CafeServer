using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Helpers;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class GiftCardController: Controller
    {
        private IGiftCardRepository _giftcards;
        private IAuthRepository _auth;
        private IRestaurantRepository _restaurant;
        private ICityRepository _city;
        private IHelper _helper;
        private ICustomerRepository _customers;
        private IGiftCardTypeRepository _giftCardTypes;
        public GiftCardController(IGiftCardRepository giftcards,
                                  IAuthRepository auth,
                                  IRestaurantRepository restaurant,
                                  ICityRepository city,
                                  IHelper helper,
                                  ICustomerRepository customers,
                                  IGiftCardTypeRepository giftCardTypes)
        {
            this._auth = auth;
            this._customers = customers;
            this._giftcards = giftcards;
            this._giftCardTypes = giftCardTypes;
            this._helper = helper;
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
                if (!this._helper.OwnesGiftCard(User, id, _giftcards, _customers, _auth))
                    return Forbid();
                var g = this._giftcards.Get(id);
                return Ok(g);
            }
            catch (NullReferenceException)
            {
                return NotFound("The record could not be found.");
            }
        }

        [HttpGet]
        [Route("getByCustomer")]
        [Authorize]
        public ActionResult GetByCustomer(int customerId)
        {
            if (!this._helper.OwnesCustomer(User, customerId, _customers, _auth))
                return Forbid();
            var roles = this.User.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();
            IEnumerable<GiftCard> giftcards;
            if (roles.Contains("Manager"))
                giftcards = this._giftcards.GetByCustomer(customerId, true).ToList();
            else
                giftcards = this._giftcards.GetByCustomer(customerId, false).ToList();
            return Ok(giftcards);
        }

        [HttpGet]
        [Route("getByRestaurant")]
        [Authorize]
        public ActionResult GetByRestaurant()
        {
            int restaurantId = this._helper.GetUserEntity(User, _auth).RestaurantId;
            var giftcards = this._giftcards.GetByRestaurant(restaurantId);
            return Ok(giftcards);
        }

        [HttpPost]
        [Route("post")]
        [Authorize]
        public ActionResult Post([FromBody] GiftCard giftcard)
        {
            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(ModelState.ToList()));
            if (!this._helper.OwnesCustomer(User, giftcard.CustomerId, this._customers, this._auth))
                return Forbid();
            giftcard.UpdatedAt = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(_helper.GetTimeZone(User, _auth, _restaurant, _city)));
            giftcard.UpdatedBy = this._helper.GetUsername(User);
            this._giftcards.Create(giftcard);
            return Ok(giftcard.Id);
        }

        [HttpPut]
        [Route("put/{id}")]
        [Authorize]
        public ActionResult Put(int id, [FromBody] GiftCard giftcard)
        {
            if (id != giftcard.Id)
                return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(ModelState.ToList()));

            if (!this._helper.OwnesGiftCard(User, id, this._giftcards, this._customers, this._auth))
                return Forbid();

            giftcard.UpdatedAt = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(_helper.GetTimeZone(User, _auth, _restaurant, _city)));
            giftcard.UpdatedBy = this._helper.GetUsername(User);
            GiftCard g = this._giftcards.Update(id, giftcard);
            return Ok(g);
        }

        [HttpPut]
        [Route("archive/{id}")]
        [Authorize]
        public ActionResult Archive(int id)
        {
            if (!this._helper.OwnesGiftCard(User, id, this._giftcards, this._customers, this._auth))
                return Forbid();
            this._giftcards.Archive(id);
            return Ok(id);
        }

        [HttpPut]
        [Route("unarchive/{id}")]
        [Authorize]
        public ActionResult Unarchive(int id)
        {
            if (!this._helper.OwnesGiftCard(User, id, this._giftcards, this._customers, this._auth))
                return Forbid();
            this._giftcards.Unarchive(id);
            return Ok(id);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles ="Manager")]
        public ActionResult Delete(int id)
        {
            if (!this._helper.OwnesGiftCard(User, id, this._giftcards, this._customers, this._auth))
                return Forbid();
            this._giftcards.Delete(id);
            return Ok(id);
        }

        [HttpGet]
        [Route("history/{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult History(int id)
        {
            if (!this._helper.OwnesGiftCard(User, id, this._giftcards, this._customers, this._auth))
                return Forbid();
            var histories = this._giftcards.GetHistory(id);
            return Ok(histories);
        }

        [HttpGet]
        [Route("types")]
        [Authorize]
        public ActionResult GetTypes()
        {
            var types = this._giftCardTypes.GetAll();
            return Ok(types);
        }

    }
}
