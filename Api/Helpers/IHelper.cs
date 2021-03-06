﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Helpers
{
    public interface IHelper
    {
        // gets the claims principal inside a controller and returns the database entity  representing the relevant user
        User GetUserEntity(ClaimsPrincipal user, IAuthRepository repo);

        // gets the user Id without querying the database, straight from the bearer token.
        string GetUserId(ClaimsPrincipal user);

        // gets the username straight from the bearer token
        string GetUsername(ClaimsPrincipal user);
        
        // Gets the errors in the modelstate object in a controller and returns a clean friendly list of errors
        List<KeyValuePair<string, string>> GetErrorsList(List<KeyValuePair<string, ModelStateEntry>> errors);

        // checks if customer belongs to the user's restaurant
        bool OwnesCustomer(ClaimsPrincipal user, int customerId, ICustomerRepository _customers, IAuthRepository _auth);
        
        // checks if the order belongs to a restaurant
        bool OwnesOrder(ClaimsPrincipal user, int orderId, IOrderRepository orderRep, ICustomerRepository customerRep, IAuthRepository auth);

        // checks if the giftcard belongs to a restaurant
        bool OwnesGiftCard(ClaimsPrincipal user, int giftCardId, IGiftCardRepository _giftCards, ICustomerRepository _customers, IAuthRepository _auth);

        // checks if the reservation belongs to a restaurant
        bool OwnesReservation(ClaimsPrincipal user, int reservationsId, IReservationRepository _reservations, ICustomerRepository _customers, IAuthRepository _auth);

        // checks if the user belongs to a restaurant
        bool OwnesUser(ClaimsPrincipal user, string userId, IAdminRepository _admin,  IAuthRepository _auth);

        string GetTimeZone(ClaimsPrincipal user, IAuthRepository auth, IRestaurantRepository restaurant, ICityRepository city);
    }
}
