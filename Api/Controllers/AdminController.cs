using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Helpers;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class AdminController: Controller
    {
        private IAuthRepository _auth;
        private IAdminRepository _adminRep;
        private IHelper _helper;

        public AdminController(IAdminRepository rep, IHelper helper, IAuthRepository auth)
        {
            this._adminRep = rep;
            this._helper = helper;
            this._auth = auth;
        }

        [HttpGet]
        [Route("getUsers")]
        [Authorize(Roles ="Manager")]
        public ActionResult GetUsers()
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            var users = _adminRep.GetUsersByRestaurant(restaurantId).Select(u => new
            {
                u.Id,
                u.MustChangePassword,
                u.Name,
                u.Username,
                u.Verified,
                u.Active,
                u.Email
            });
            return Ok(users);
        }

        [HttpGet]
        [Route("getUser/{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult GetUser(string id)
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;

            if (_helper.OwnesUser(User, id, _adminRep, _auth))
            {

                var users = _adminRep.GetUsersByRestaurant(restaurantId).Select(u => new
                {
                    u.Id,
                    u.MustChangePassword,
                    u.Name,
                    u.Username,
                    u.Verified,
                    u.Active,
                    u.Email
                });
                return Ok(users);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("activateUser/{id}")]
        [Authorize(Roles ="Manager")]
        public ActionResult ActivateUser(string id)
        {
            if (_helper.OwnesUser(User, id, _adminRep, _auth))
            {
                var user = this._adminRep.GetUser(id);
                user.Active = true;
                this._adminRep.UpdateUser(id, user);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("deactivateUser/{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult DeactivateUser(string id)
        {
            if (_helper.OwnesUser(User, id, _adminRep, _auth))
            {
                var user = this._adminRep.GetUser(id);
                user.Active = false;
                this._adminRep.UpdateUser(id, user);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("updatePassword")]
        [Authorize]
        public ActionResult UpdatePassword(string oldPassword, string currentPassword)
        {
            var user = _adminRep.GetUser(_helper.GetUserId(User));
            PasswordHasher
        }
    }
}
