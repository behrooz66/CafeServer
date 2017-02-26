﻿using System;
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
    public class GiftCardController: Controller
    {
        private IGiftCardRepository _giftcards;
        private IAuthRepository _auth;
        private IHelper _helper;
        private ICustomerRepository _customers;
        private IGiftCardTypeRepository _giftCardTypes;
        public GiftCardController(IGiftCardRepository giftcards,
                                  IAuthRepository auth,
                                  IHelper helper,
                                  ICustomerRepository customers,
                                  IGiftCardTypeRepository giftCardTypes)
        {
            this._auth = auth;
            this._customers = customers;
            this._giftcards = giftcards;
            this._giftCardTypes = giftCardTypes;
            this._helper = helper;
        }

        [HttpGet]
        [Route("get/{id}")]
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
        public ActionResult GetByCustomer(int customerId)
        {
            if (!this._helper.OwnesCustomer(User, customerId, _customers, _auth))
                return Forbid();
            var giftcards = this._giftcards.GetByCustomer(customerId).ToList();
            return Ok(giftcards);
        }

        [HttpGet]
        [Route("getByRestaurant")]
        public ActionResult GetByRestaurant()
        {
            int restaurantId = this._helper.GetUserEntity(User, _auth).RestaurantId;
            var giftcards = this._giftcards.GetByRestaurant(restaurantId);
            return Ok(giftcards);
        }

        [HttpPost]
        [Route("post")]
        public ActionResult Post([FromBody] GiftCard giftcard)
        {
            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(ModelState.ToList()));
            if (!this._helper.OwnesCustomer(User, giftcard.CustomerId, this._customers, this._auth))
                return Forbid();
            giftcard.UpdatedAt = DateTime.Now;
            giftcard.UpdatedBy = this._helper.GetUsername(User);
            this._giftcards.Create(giftcard);
            return Ok(giftcard.Id);
        }

        [HttpPut]
        [Route("put/{id}")]
        public ActionResult Put(int id, [FromBody] GiftCard giftcard)
        {
            if (id != giftcard.Id)
                return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(ModelState.ToList()));

            if (!this._helper.OwnesGiftCard(User, id, this._giftcards, this._customers, this._auth))
                return Forbid();

            GiftCard g = this._giftcards.Update(id, giftcard);
            return Ok(g);
        }

        [HttpPut]
        [Route("archive/{id}")]
        public ActionResult Archive(int id)
        {
            if (!this._helper.OwnesGiftCard(User, id, this._giftcards, this._customers, this._auth))
                return Forbid();
            this._giftcards.Archive(id);
            return Ok(id + " archived.");
        }

        [HttpGet]
        [Route("types")]
        public ActionResult GetTypes()
        {
            var types = this._giftCardTypes.GetAll();
            return Ok(types);
        }

    }
}
