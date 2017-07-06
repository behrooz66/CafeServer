using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Helpers
{
    public class Helper : IHelper
    {
        public List<KeyValuePair<string, string>> GetErrorsList(List<KeyValuePair<string, ModelStateEntry>> errors)
        {
            var result = new List<KeyValuePair<string, string>>();
            foreach (var err in errors)
            {
                foreach (var e in err.Value.Errors)
                {
                    result.Add(new KeyValuePair<string, string>(err.Key, e.ErrorMessage));
                }
            }
            return result;
        }

        public User GetUserEntity(ClaimsPrincipal user, IAuthRepository repo)
        {
            var uid = user.Identities.FirstOrDefault().Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var User = repo.GetUserById(uid);
            return User;
        }


        // todo: uncomment the real line!
        public string GetUserId(ClaimsPrincipal user)
        {
            //var uid = user.Identities.FirstOrDefault().Claims.Where(c => c.Type == "sub").FirstOrDefault().Value;
            var uid = "0000-!";
            return uid;
        }

        public string GetUsername(ClaimsPrincipal user)
        {
            // todo: uncomment the real line!
            // var uname = user.Identities.FirstOrDefault().Claims.Where(c => c.Type == "preferred_username").FirstOrDefault().Value;
            var uname = "behrooz!";
            return uname;
        }

        public bool OwnesCustomer(ClaimsPrincipal user, int customerId, 
                                    ICustomerRepository _customers, IAuthRepository _auth)
        {
            if (_customers.Get(customerId).RestaurantId == this.GetUserEntity(user, _auth).RestaurantId)
                return true;
            return false;
        }

        public bool OwnesGiftCard(ClaimsPrincipal user, int giftCardId, 
                                  IGiftCardRepository _giftCards, ICustomerRepository _customers,
                                  IAuthRepository _auth)
        {
            var customerId = _giftCards.Get(giftCardId).CustomerId;
            var restaurantId = _customers.Get(customerId).RestaurantId;
            if (this.GetUserEntity(user, _auth).RestaurantId == restaurantId)
                return true;
            return false;
        }

        public bool OwnesOrder(ClaimsPrincipal user, int orderId, 
                               IOrderRepository repOrder, ICustomerRepository repCustomer,
                               IAuthRepository auth)
        {
            var customerId = repOrder.Get(orderId).CustomerId;
            var restaurantId = repCustomer.Get(customerId).RestaurantId;
            if (this.GetUserEntity(user, auth).RestaurantId == restaurantId)
                return true;
            return false;
        }

        public bool OwnesReservation(ClaimsPrincipal user, int reservationsId, IReservationRepository _reservations, ICustomerRepository _customers, IAuthRepository _auth)
        {
            var customerId = _reservations.Get(reservationsId).CustomerId;
            var restaurantId = _customers.Get(customerId).RestaurantId;
            if (this.GetUserEntity(user, _auth).RestaurantId == restaurantId)
                return true;
            return false;
        }

        public bool OwnesUser(ClaimsPrincipal user, string userId, IAdminRepository _admin,  IAuthRepository _auth)
        {
            var usersRestaurant = _admin.GetUser(userId).RestaurantId;
            var ownersRestaurant = this.GetUserEntity(user, _auth).RestaurantId;
            if (usersRestaurant == ownersRestaurant)
                return true;
            return false;
        }
    }
}
