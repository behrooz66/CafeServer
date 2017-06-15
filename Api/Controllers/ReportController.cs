using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using AuthServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Api.Helpers;
using System.Dynamic;

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
        private IGiftCardRepository _giftCards;
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






        // Garbage:
        /*[Route("reservationsMonthlySum2")]
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult ReservationsMonthlySum2(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            var startDate = dateFrom ?? _reservations.GetByRestaurant(restaurantId).Last().Date;
            var endDate = dateTo ?? _reservations.GetByRestaurant(restaurantId).First().Date;
            string[] months = GetMonthsArray(startDate, endDate);
            dynamic[] res = new dynamic[months.Length];
            for (int i = 0; i < months.Length; i++)
            {
                res[i] = new ExpandoObject();
                var year = int.Parse(months[i].Substring(0, 4));
                var month = int.Parse(months[i].Substring(5, 2));
                res[i].Month = months[i];

                res[i].Revenue = _reservations.GetByRestaurant(restaurantId).Where(r => r.Date.Year == year && r.Date.Month == month).Where(r => r.ReservationStatusId == 1);
            }
            return Ok();
        }*/
    }
}
