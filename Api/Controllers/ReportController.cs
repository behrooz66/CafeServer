﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using AuthServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Api.Helpers;
using System.Dynamic;
using System.Collections;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class ReportController: Controller
    {
        private IOrderRepository _orders;
        private IReservationRepository _reservations;
        private IGiftCardRepository _giftcards;
        private IAuthRepository _auth;
        private ICustomerRepository _customers;
        private IOrderTypeRepository _orderTypes;
        private IReservationStatusRepository _reservationStatuses;
        private IGiftCardTypeRepository _giftCardTypes;
        private IHelper _helper;

        public ReportController(IOrderRepository orders,
                                IReservationRepository reservations,
                                IGiftCardRepository giftcards,
                                IAuthRepository auth,
                                ICustomerRepository customers,
                                IOrderTypeRepository orderTypes,
                                IReservationStatusRepository reservationStatuses, 
                                IGiftCardRepository giftCards,
                                IGiftCardTypeRepository giftCardTypes,
                                IHelper helper)
        {
            this._orders = orders;
            this._reservations = reservations;
            this._giftcards = giftcards;
            this._helper = helper;
            this._auth = auth;
            this._customers = customers;
            this._orderTypes = orderTypes;
            this._reservationStatuses = reservationStatuses;
            this._giftcards = giftcards;
            this._giftCardTypes = giftCardTypes;
        }


        #region Monthly Summaries
        // MONTHLY SUMMARIES 

        [Route("ordersMonthlySum")]
        [HttpGet]
        [Authorize(Roles ="Manager")]
        public ActionResult OrdersMonthlySum(DateTime? dateFrom = null, DateTime? dateTo = null, int? typeId = null)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            //var customerIds = _customers.GetByRestaurant(restaurantId, false).Select(c => c.Id).ToList();
            var startDate = dateFrom ?? _orders.GetByRestaurant(restaurantId).Last().Date;
            var endDate = dateTo ?? _orders.GetByRestaurant(restaurantId).First().Date;
            string[] months = GetMonthsArray(startDate, endDate);
            dynamic result = new dynamic[months.Length];
            var orderTypes = _orderTypes.GetAll().ToArray();



            for (int i=0; i<months.Length; i++)
            {
                result[i] = new ExpandoObject();
                result[i].Types = new dynamic[orderTypes.Count()];
                for (var x = 0; x < orderTypes.Count(); x++)
                {
                    result[i].Types[x] = new ExpandoObject();
                }

                var year = int.Parse(months[i].Substring(0, 4));
                var month = int.Parse(months[i].Substring(5, 2));
                result[i].Month = months[i];
                result[i].TotalOrders = 0;
                result[i].TotalRevenue = 0;

                for (var x = 0; x < orderTypes.Count(); x++)
                {
                    var oId = orderTypes[x].Id;
                    var oName = orderTypes[x].Type;
                    result[i].Types[x].Type = oName;
                    result[i].Types[x].Revenue = _orders.GetByRestaurant(restaurantId).Where(o => o.OrderTypeId == oId)
                                                 .Where(o => o.Date.Year == year && o.Date.Month == month).Sum(o => (decimal?)o.Price) ?? 0;

                    result[i].Types[x].Count = _orders.GetByRestaurant(restaurantId).Where(o => o.OrderTypeId == oId)
                                                 .Where(o => o.Date.Year == year && o.Date.Month == month).Count();
                    
                    result[i].TotalOrders += result[i].Types[x].Count;
                    result[i].TotalRevenue += result[i].Types[x].Revenue;
                }
            }

            return Ok(result);
        }


        [Route("reservationsMonthlySum")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult ReservationsMonthlySum(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            
            var startDate = dateFrom ?? _reservations.GetByRestaurant(restaurantId).Last().Date;
            var endDate = dateTo ?? _reservations.GetByRestaurant(restaurantId).First().Date;
            string[] months = GetMonthsArray(startDate, endDate);
            dynamic result = new dynamic[months.Length];
            var reservationStatuses = _reservationStatuses.GetAll().ToArray();

            for (int i = 0; i < months.Length; i++)
            {
                result[i] = new ExpandoObject();
                result[i].Statuses = new dynamic[reservationStatuses.Count()];
                for (var x = 0; x < reservationStatuses.Count(); x++)
                {
                    result[i].Statuses[x] = new ExpandoObject();
                }

                var year = int.Parse(months[i].Substring(0, 4));
                var month = int.Parse(months[i].Substring(5, 2));
                result[i].Month = months[i];
                result[i].TotalReservations = 0;
                result[i].TotalRevenue = 0;

                for (var x = 0; x < reservationStatuses.Count(); x++)
                {
                    var oId = reservationStatuses[x].Id;
                    var oName = reservationStatuses[x].Status;
                    result[i].Statuses[x].Status = oName;
                    result[i].Statuses[x].Revenue = _reservations.GetByRestaurant(restaurantId).Where(o => o.ReservationStatusId == oId)
                                                .Where(o => o.Date.Year == year && o.Date.Month == month).Sum(o => (decimal?)o.Revenue) ?? 0;
                    result[i].Statuses[x].Count = _reservations.GetByRestaurant(restaurantId).Where(o => o.ReservationStatusId == oId)
                                                 .Where(o => o.Date.Year == year && o.Date.Month == month).Count();
                    result[i].TotalReservations += result[i].Statuses[x].Count;
                    result[i].TotalRevenue += result[i].Statuses[x].Revenue;
                }
            }
            return Ok(result);
        }


        [Route("giftCardsMonthlySum")]
        [HttpGet]
        [Authorize (Roles = "Manager")]
        public ActionResult GiftCardsMonthlySum(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            var startDate = dateFrom ?? _giftcards.GetByRestaurant(restaurantId).Last().IssueDate;
            var endDate = dateTo ?? _giftcards.GetByRestaurant(restaurantId).First().IssueDate;
            string[] months = GetMonthsArray(startDate, endDate);
            dynamic result = new dynamic[months.Length];
            var giftCardTypes = _giftCardTypes.GetAll().ToArray();
            for (int i = 0; i < months.Length; i++)
            {
                result[i] = new ExpandoObject();
                result[i].Types = new dynamic[giftCardTypes.Count()];
                for (var x = 0; x < giftCardTypes.Count(); x++)
                {
                    result[i].Types[x] = new ExpandoObject();
                }
                var year = int.Parse(months[i].Substring(0, 4));
                var month = int.Parse(months[i].Substring(5, 2));
                result[i].Month = months[i];
                result[i].TotalGiftCards = 0;
                result[i].TotalRevenue = 0;
                for (var x = 0; x < giftCardTypes.Count(); x++)
                {
                    var oId = giftCardTypes[x].Id;
                    var oName = giftCardTypes[x].Type;
                    result[i].Types[x].Type = oName;
                    result[i].Types[x].Revenue = _giftcards.GetByRestaurant(restaurantId).Where(o => o.GiftCardTypeId == oId)
                                                .Where(o => o.IssueDate.Year == year && o.IssueDate.Month == month).Sum(o => (decimal?)o.Amount) ?? 0;
                    result[i].Types[x].Count = _giftcards.GetByRestaurant(restaurantId).Where(o => o.GiftCardTypeId == oId)
                                               .Where(o => o.IssueDate.Year == year && o.IssueDate.Month == month).Count();
                    result[i].TotalGiftCards += result[i].Types[x].Count;
                    result[i].TotalRevenue += result[i].Types[x].Revenue;
                }
            }
            return Ok(result);
        }

        // MONTHLY SUMMARIES end.
        #endregion

        #region Record Details
        // RECORD DETAILS 

        [Route("orderRecords")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult OrderRecords(DateTime? dateFrom = null, DateTime? dateTo = null, int? typeId = null, decimal? priceFrom = null, decimal? priceTo = null, bool deleted = false)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            var orders = _orders.GetByRestaurant(restaurantId).Select(x => new
            {
                x.Id,
                x.OrderTypeId,
                OrderType = _orderTypes.GetAll().SingleOrDefault(s => s.Id == x.OrderTypeId).Type,
                Customer = _customers.Get(x.CustomerId).Name,
                x.Date,
                x.Price,
                x.UpdatedAt,
                x.UpdatedBy,
                x.Notes,
                Lat = _customers.Get(x.CustomerId).Lat,
                Lon = _customers.Get(x.CustomerId).Lon,
                NoAddress = _customers.Get(x.CustomerId).NoAddress || !_customers.Get(x.CustomerId).AddressFound,
                x.Deleted
            });

            if (dateFrom != null) orders = orders.Where(o => o.Date >= dateFrom);
            if (dateTo != null) orders = orders.Where(o => o.Date <= dateTo);
            if (typeId != null && typeId != 0) orders = orders.Where(o => o.OrderTypeId == typeId);
            if (priceFrom != null) orders = orders.Where(o => o.Price >= priceFrom);
            if (priceTo != null) orders = orders.Where(o => o.Price <= priceFrom);
            if (!deleted) orders = orders.Where(o => !o.Deleted);

            var O = orders.ToList();
            return Ok(O);
        }

        [Route("reservationRecords")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult ReservationRecords(DateTime? dateFrom = null, DateTime? dateTo = null, int? statusId = null, bool deleted = false)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            var reservations = _reservations.GetByRestaurant(restaurantId).Select(x => new
            {
                x.Id,
                x.ReservationStatusId,
                ReservationStatus = _reservationStatuses.GetAll().SingleOrDefault(s => s.Id == x.ReservationStatusId).Status,
                Customer = _customers.Get(x.CustomerId).Name,
                x.Date,
                x.Time,
                x.Revenue,
                x.UpdatedAt,
                x.UpdatedBy,
                x.Notes,
                Lat = _customers.Get(x.CustomerId).Lat,
                Lon = _customers.Get(x.CustomerId).Lon,
                NoAddress = _customers.Get(x.CustomerId).NoAddress || !_customers.Get(x.CustomerId).AddressFound,
                x.Deleted
            });
            if (dateFrom != null) reservations = reservations.Where(o => o.Date >= dateFrom);
            if (dateTo != null) reservations = reservations.Where(o => o.Date <= dateTo);
            if (statusId != null && statusId != 0) reservations = reservations.Where(o => o.ReservationStatusId == statusId);
            if (!deleted) reservations = reservations.Where(o => !o.Deleted);

            var R = reservations.ToList();
            return Ok(R);
        }

        [Route("giftCardRecords")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult GiftCardRecords(decimal? amountFrom = null, decimal? amountTo = null,
                                            DateTime? issueDateFrom = null, DateTime? issueDateTo = null, DateTime? expiryDateFrom = null, DateTime? expiryDateTo = null,
                                            int? typeId = null, bool deleted = false)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            var giftCards = _giftcards.GetByRestaurant(restaurantId).Select(x => new
            {
                x.Id,
                x.GiftCardTypeId,
                GiftCardType = _giftCardTypes.GetAll().SingleOrDefault(s => s.Id == x.GiftCardTypeId).Type,
                Customer = _customers.Get(x.CustomerId).Name,
                x.Number,
                x.IssueDate,
                x.ExpiryDate,
                x.Amount,
                x.UpdatedAt,
                x.UpdatedBy,
                x.Notes,
                Lat = _customers.Get(x.CustomerId).Lat,
                Lon = _customers.Get(x.CustomerId).Lon,
                NoAddress = _customers.Get(x.CustomerId).NoAddress || !_customers.Get(x.CustomerId).AddressFound,
                x.Deleted
            });

            if (amountFrom != null) giftCards = giftCards.Where(g => g.Amount >= amountFrom);
            if (amountTo != null) giftCards = giftCards.Where(g => g.Amount <= amountTo);
            if (typeId != null && typeId != 0) giftCards.Where(g => g.GiftCardTypeId == typeId);
            if (issueDateFrom != null) giftCards.Where(g => g.IssueDate >= issueDateFrom);
            if (issueDateTo != null) giftCards.Where(g => g.IssueDate <= issueDateTo);
            if (expiryDateFrom != null) giftCards.Where(g => g.ExpiryDate >= expiryDateFrom);
            if (expiryDateTo != null) giftCards.Where(g => g.ExpiryDate <= expiryDateTo);
            if (!deleted) giftCards = giftCards.Where(g => !g.Deleted);

            var G = giftCards.ToList();
            return Ok(G);
        }

        // RECORD DETAILS end.
        #endregion

        #region Top Customers

        [Route("ordersTopCustomers")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult OrdersTopCustomers(DateTime? dateFrom = null, DateTime? dateTo = null, double? minRevenue = 0, int? minOrder = 1)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            var startDate = dateFrom ?? _orders.GetByRestaurant(restaurantId).Last().Date;
            var endDate = dateTo ?? _orders.GetByRestaurant(restaurantId).First().Date;

            try
            {
                var result = _customers.GetByRestaurant(restaurantId, false).Select(x => new
                {
                    x.Id,
                    x.Name,
                    Orders = _orders.GetByCustomer(x.Id, false).Where(o => o.Date >= startDate && o.Date <= endDate)
                                                               .Count(),
                    Revenue = _orders.GetByCustomer(x.Id, false).Where(o => o.Date >= startDate && o.Date <= endDate)
                                                                .Sum(o => (decimal?)o.Price) ?? 0,
                    LastOrderOn = _orders.GetByCustomer(x.Id, false)
                                    .Where(o => o.Date >= startDate && o.Date <= endDate).Count() > 0 ?
                                  _orders.GetByCustomer(x.Id, false)
                                  .Where(o => o.Date >= startDate && o.Date <= endDate)
                                  .OrderByDescending(o => o.Date)
                                  .FirstOrDefault().Date.ToString() : "Never"
                            }).Where(o => o.Orders >= minOrder)
                          .Where(o => (double)o.Revenue >= minRevenue)
                          .OrderByDescending(o => o.Revenue).ToList();
                                    return Ok(result);
            }
            catch (NullReferenceException)
            {
                return NoContent();
            }
        }

        [Route("reservationsTopCustomers")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult ReservationsTopCustomers(DateTime? dateFrom = null, DateTime? dateTo = null,
                                                     double? minRevenue = 0, int? minReservation = 1, bool includeUnspecified = false)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            var startDate = dateFrom ?? _reservations.GetByRestaurant(restaurantId).Last().Date;
            var endDate = dateTo ?? _reservations.GetByRestaurant(restaurantId).First().Date;

            var result = _customers.GetByRestaurant(restaurantId, false).Select(x => new
            {
                x.Id,
                x.Name,
                // we have to do a bit of hardcoding for statuses here (against my will). We exclude status 2 and 3.
                //For 4 (unspecified), we conditionally exclude it. 0 is just there to help us write the statement.
                Reservations = _reservations.GetByCustomer(x.Id, false).Where(o => o.Date >= startDate && o.Date <= endDate)
                                                                           .Where(r => r.ReservationStatusId != 2 && r.ReservationStatusId != 3)
                                                                           .Where(r => r.ReservationStatusId != (includeUnspecified ? 0 : 4))
                                                                           .Count(),
                Revenue = _reservations.GetByCustomer(x.Id, false).Where(o => o.Date >= startDate && o.Date <= endDate)
                                                                      .Where(r => r.ReservationStatusId != 2 && r.ReservationStatusId != 3)
                                                                      .Where(r => r.ReservationStatusId != (includeUnspecified ? 0 : 4))
                                                                      .Sum(o => (decimal?)o.Revenue) ?? 0,
                LastReservationOn = _reservations.GetByCustomer(x.Id, false)
                                        .Where(o => o.Date >= startDate && o.Date <= endDate)
                                        .Where(r => r.ReservationStatusId != 2 && r.ReservationStatusId != 3)
                                        .Where(r => r.ReservationStatusId != (includeUnspecified ? 0 : 4))
                                        .Count() > 0 ?
                                _reservations.GetByCustomer(x.Id, false).Where(o => o.Date >= startDate && o.Date <= endDate)
                                    .Where(r => r.ReservationStatusId != 2 && r.ReservationStatusId != 3)
                                    .Where(r => r.ReservationStatusId != (includeUnspecified ? 0 : 4))
                                    .OrderByDescending(o => o.Date)
                                    .FirstOrDefault().Date.ToString() : "Never"
            }).Where(o => o.Reservations >= minReservation)
                  .Where(o => (double)o.Revenue >= minRevenue)
                  .OrderByDescending(o => o.Revenue).ToList();
            return Ok(result);
        }

        [Route("giftCardsTopCustomers")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult GiftCardsTopCustomers(DateTime? dateFrom = null, DateTime? dateTo = null, double? minRevenue = 0, int? minCards = 1, int? typeId = null)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            var startDate = dateFrom ?? _giftcards.GetByRestaurant(restaurantId).Last().IssueDate;
            var endDate = dateTo ?? _giftcards.GetByRestaurant(restaurantId).First().IssueDate;
            var allTypeIds = _giftCardTypes.GetAll().Select(t => t.Id).ToList();
            var acceptedIds = new List<int>();

            if (typeId == null || typeId == 0)
                acceptedIds = allTypeIds;
            else
            {
                acceptedIds.Clear();
                acceptedIds.Add((int)typeId);
            }

            
            try
            {
                var kir = _giftcards.GetByCustomer(1, false).ToList();

                var result = _customers.GetByRestaurant(restaurantId, false).Select(x => new
                {
                    x.Id,
                    x.Name,
                    GiftCards = _giftcards.GetByCustomer(x.Id, false)
                                   .Where(o => o.IssueDate >= startDate && o.IssueDate <= endDate)
                                   .Where(o => acceptedIds.Contains(o.GiftCardTypeId))
                                   .Count(),
                    Revenue = _giftcards.GetByCustomer(x.Id, false)
                                   .Where(o => o.IssueDate >= startDate && o.IssueDate <= endDate)
                                   .Where(o => acceptedIds.Contains(o.GiftCardTypeId))
                                   .Sum(o => (decimal?)o.Amount) ?? 0,
                    LastGiftCardrOn = _giftcards.GetByCustomer(x.Id, false)
                                        .Where(o => o.IssueDate >= startDate && o.IssueDate <= endDate)
                                        .Where(o => acceptedIds.Contains(o.GiftCardTypeId))
                                        .Count() > 0 ?
                                _giftcards.GetByCustomer(x.Id, false)
                                .Where(o => o.IssueDate >= startDate && o.IssueDate <= endDate)
                                .Where(o => acceptedIds.Contains(o.GiftCardTypeId))
                                .OrderByDescending(o => o.IssueDate)
                                .FirstOrDefault().IssueDate.ToString() : "Never"
                }).Where(o => o.GiftCards >= minCards)
                  .Where(o => (double)o.Revenue >= minRevenue)
                  .OrderByDescending(o => o.Revenue).ToList();
                return Ok(result);
            }
            catch(NullReferenceException)
            {
                return NoContent();
            }
            
        }

        #endregion

        #region Daily Summaries
        // DAILY SUMMARIES

        [Route("ordersDailySum")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult ordersDailySum(DateTime? dateFrom = null, DateTime? dateTo = null, int? typeId = null)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            try
            {
                var startDate = dateFrom ?? _orders.GetByRestaurant(restaurantId).OrderBy(o => o.Date).First().Date;
                var endDate = dateTo ?? _orders.GetByRestaurant(restaurantId).OrderBy(o => o.Date).Last().Date;
                var types = _orderTypes.GetAll().ToList();
                var acceptedTypes = types.ToList();
                acceptedTypes.Clear();
                if (typeId == null || typeId == 0)
                {
                    acceptedTypes = types;
                }
                else
                {

                    acceptedTypes.Add(_orderTypes.Get((int)typeId));
                }
                var x = _orders.GetByRestaurant(restaurantId)
                        .Where(o => o.Date >= startDate && o.Date <= endDate)
                        .Select(o => o.Date.Date)
                        .OrderBy(o => o.Date)
                        .Distinct().ToList();

                dynamic[] result = new dynamic[x.Count()];
                for (int i = 0; i < x.Count(); i++)
                {
                    result[i] = new ExpandoObject();
                    result[i].Date = x[i].Date;
                    foreach (var t in acceptedTypes)
                    {
                        ((IDictionary<String, Object>)result[i])[t.Type.Replace(" ", "") + "_revenue"] =
                            _orders.GetByRestaurant(restaurantId)
                                   .Where(g => g.Date.Date == result[i].Date.Date)
                                   .Where(g => g.OrderTypeId == t.Id)
                                   .Sum(g => g.Price);

                        ((IDictionary<String, Object>)result[i])[t.Type.Replace(" ", "") + "_number"] =
                             _orders.GetByRestaurant(restaurantId)
                                   .Where(g => g.Date.Date == result[i].Date.Date)
                                   .Where(g => g.OrderTypeId == t.Id)
                                   .Count();
                    }
                    
                }
                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NoContent();
            }
            catch (NullReferenceException)
            {
                return NoContent();
            }
        }

        [Route("reservationsDailySum")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult reservationsDailySum(DateTime? dateFrom = null, DateTime? dateTo = null, int? statusId = null)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            try
            {
                var startDate = dateFrom ?? _reservations.GetByRestaurant(restaurantId).OrderBy(o => o.Date).First().Date;
                var endDate = dateTo ?? _reservations.GetByRestaurant(restaurantId).OrderBy(o => o.Date).Last().Date;
                var statuses = _reservationStatuses.GetAll().ToList();
                var acceptedStatuses = statuses.ToList();
                acceptedStatuses.Clear();
                if (statusId == null || statusId == 0)
                {
                    acceptedStatuses = statuses;
                }
                else
                {
                    acceptedStatuses.Add(_reservationStatuses.Get((int)statusId));
                }
                var x = _reservations.GetByRestaurant(restaurantId)
                        .Where(r => r.Date >= startDate && r.Date <= endDate)
                        .Select(r => r.Date.Date)
                        .OrderBy(r => r.Date)
                        .Distinct().ToList();

                dynamic[] result = new dynamic[x.Count()];
                for (int i = 0; i < x.Count(); i++)
                {
                    result[i] = new ExpandoObject();
                    result[i].Date = x[i].Date;
                    foreach (var s in acceptedStatuses)
                    {
                        ((IDictionary<String, Object>)result[i])[s.Status.Replace(" ", "") + "_revenue"] =
                                _reservations.GetByRestaurant(restaurantId)
                                        .Where(g => g.Date.Date == result[i].Date.Date)
                                        .Where(g => g.ReservationStatusId == s.Id)
                                        .Sum(g => g.Revenue);

                        ((IDictionary<String, Object>)result[i])[s.Status.Replace(" ", "") + "_number"] =
                                _reservations.GetByRestaurant(restaurantId)
                                        .Where(g => g.Date.Date == result[i].Date.Date)
                                        .Where(g => g.ReservationStatusId == s.Id)
                                        .Count();
                    }
                }
                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NoContent();
            }
            catch (NullReferenceException)
            {
                return NoContent();
            }
        }

        [Route("giftCardsDailySum")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult giftCardsDailySum(DateTime? dateFrom = null, DateTime? dateTo = null, int? typeId = null)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            try
            {
                var startDate = dateFrom ?? _giftcards.GetByRestaurant(restaurantId).OrderBy(o => o.IssueDate).First().IssueDate;
                var endDate = dateTo ?? _giftcards.GetByRestaurant(restaurantId).OrderBy(o => o.IssueDate).Last().IssueDate;
                var types = _giftCardTypes.GetAll().ToList();
                var acceptedTypes = types.ToList();
                acceptedTypes.Clear();
                if (typeId == null || typeId == 0)
                    acceptedTypes = types;
                else
                    acceptedTypes.Add(_giftCardTypes.Get((int)typeId));
                var x = _giftcards.GetByRestaurant(restaurantId)
                        .Where(g => g.IssueDate >= startDate && g.IssueDate <= endDate)
                        .Select(g => g.IssueDate.Date)
                        .OrderBy(g => g.Date)
                        .Distinct().ToList();
                dynamic[] result = new dynamic[x.Count()];
                for (int i = 0; i < x.Count(); i++)
                {
                    result[i] = new ExpandoObject();
                    result[i].Date = x[i].Date;
                    foreach (var t in acceptedTypes)
                    {
                        ((IDictionary<String, Object>)result[i])[t.Type.Replace(" ", "") + "_revenue"] =
                            _giftcards.GetByRestaurant(restaurantId)
                                    .Where(g => g.IssueDate.Date == result[i].Date.Date)
                                    .Where(g => g.GiftCardTypeId == t.Id)
                                    .Sum(g => g.Amount);

                        ((IDictionary<String, Object>)result[i])[t.Type.Replace(" ", "") + "_number"] =
                            _giftcards.GetByRestaurant(restaurantId)
                                    .Where(g => g.IssueDate.Date == result[i].Date.Date)
                                    .Where(g => g.GiftCardTypeId == t.Id)
                                    .Count();
                    }
                }
                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NoContent();
            }
            catch (NullReferenceException)
            {
                return NoContent();
            }
        }

        // DAILY SUMMARIES end
        #endregion

        // internal helpers
        private string[] GetMonthsArray(DateTime start, DateTime end)
        {
            int numberOfMonths = ((end.Year - start.Year) * 12) + end.Month - start.Month + 1;
            string[] months = new string[numberOfMonths];
            for (int i = 0; i < numberOfMonths; i++)
            {
                months[i] = start.ToString("yyyy-MM");
                start = start.AddMonths(1);
            };
            return months;
        }
        
    }
}
