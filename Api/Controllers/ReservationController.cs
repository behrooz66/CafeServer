﻿using System;
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
    public class ReservationController: Controller
    {
        private IReservationRepository _reservations;
        private IAuthRepository _auth;
        private IHelper _helper;
        private ICustomerRepository _customers;
        private IReservationStatusRepository _statuses;
        public ReservationController(IReservationRepository res,
                                     IAuthRepository auth,
                                     IHelper helper,
                                     ICustomerRepository customers,
                                     IReservationStatusRepository statuses)
        {
            this._auth = auth;
            this._customers = customers;
            this._helper = helper;
            this._reservations = res;
            this._statuses = statuses;
        }

        [HttpGet]
        [Route("get/{id}")]
        [Authorize]
        public ActionResult Get(int id)
        {
            try
            {
                if (!this._helper.OwnesReservation(User, id, _reservations, _customers, _auth))
                    return Forbid();
                var r = this._reservations.Get(id);
                return Ok(r);
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
            var reservations = this._reservations.GetByCustomer(customerId).ToList();
            return Ok(reservations);
        }

        [HttpGet]
        [Route("getByRestaurant")]
        [Authorize]
        public ActionResult GetByRestaurant()
        {
            int restaurantId = this._helper.GetUserEntity(User, _auth).RestaurantId;
            var reservations = this._reservations.GetByRestaurant(restaurantId);
            return Ok(reservations);
        }

        [HttpPost]
        [Route("post")]
        [Authorize]
        public ActionResult Post([FromBody] Reservation reservation)
        {
            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(ModelState.ToList()));
            if (!this._helper.OwnesCustomer(User, reservation.CustomerId, this._customers, this._auth))
                return Forbid();
            reservation.UpdatedAt = DateTime.Now;
            reservation.UpdatedBy = this._helper.GetUsername(User);
            this._reservations.Create(reservation);
            return Ok(reservation.Id);
        }

        [HttpPut]
        [Route("put/{id}")]
        [Authorize]
        public ActionResult Put(int id, [FromBody] Reservation reservation)
        {
            if (id != reservation.Id)
                return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(this._helper.GetErrorsList(ModelState.ToList()));

            if (!this._helper.OwnesReservation(User, id, this._reservations, this._customers, this._auth))
                return Forbid();

            reservation.UpdatedAt = DateTime.Now;
            reservation.UpdatedBy = this._helper.GetUsername(User);

            Reservation r = this._reservations.Update(id, reservation);
            return Ok(r);
        }

        [HttpPut]
        [Route("archive/{id}")]
        [Authorize]
        public ActionResult Archive(int id)
        {
            if (!this._helper.OwnesReservation(User, id, this._reservations, this._customers, this._auth))
                return Forbid();
            this._reservations.Archive(id);
            return Ok(id + " archived.");
        }

        [HttpGet]
        [Route("history/{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult History(int id)
        {
            if (!this._helper.OwnesReservation(User, id, this._reservations, this._customers, this._auth))
                return Forbid();
            var histories = this._reservations.GetHistory(id);
            return Ok(histories);
        }

        [HttpGet]
        [Route("statuses")]
        [Authorize]
        public ActionResult GetStatuses()
        {
            var statuses = this._statuses.GetAll();
            return Ok(statuses);
        }
    }
}
