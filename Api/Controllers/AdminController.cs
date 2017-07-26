using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Helpers;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CryptoHelper;

using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Net.Http;

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

                var user = _adminRep.GetUser(id);
                var obj = new
                {
                    user.Name,
                    user.Email,
                    user.Active,
                    user.Id,
                    user.Username,
                    user.Verified
                };
                return Ok(obj);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("userExists/{username}")]
        [Authorize(Roles = "Manager")]
        public ActionResult UserExists(string username)
        {
            if (_adminRep.UserExists(username))
                return Ok(new {usernameExists = true });
            return Ok(new { usernameExists = false });
        }


        [HttpPut]
        [Route("activateUser/{userid}")]
        [Authorize(Roles ="Manager")]
        public ActionResult ActivateUser(string userid)
        {
            if (_helper.OwnesUser(User, userid, _adminRep, _auth))
            {
                var user = this._adminRep.GetUser(userid);
                user.Active = true;
                this._adminRep.UpdateUser(userid, user);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("deactivateUser/{userid}")]
        [Authorize(Roles = "Manager")]
        public ActionResult DeactivateUser(string userid)
        {
            if (_helper.OwnesUser(User, userid, _adminRep, _auth))
            {
                var user = this._adminRep.GetUser(userid);
                user.Active = false;
                this._adminRep.UpdateUser(userid, user);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("updatePassword")]
        [Authorize]
        public ActionResult UpdatePassword([FromBody] UpdatePasswordVM vm)
        {
            //var user = _adminRep.GetUser(_helper.GetUserId(User));
            var user = _helper.GetUserEntity(User, _auth);

            if (Crypto.VerifyHashedPassword(user.Password, vm.OldPassword))
            {
                user.Password = Crypto.HashPassword(vm.NewPassword);
                user.MustChangePassword = false;
                _adminRep.UpdateUser(user.Id, user);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("addUser")]
        [Authorize(Roles ="Manager")]
        public ActionResult AddUser([FromBody] AddUserVM user)
        {
            if (!_adminRep.UserExists(user.Username))
            {
                var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
                //var hashedPassword = Crypto.HashPassword(user.Password);
                var tempPass = Guid.NewGuid().ToString().Substring(0, 6);
                var hashedPassword = Crypto.HashPassword(tempPass);
                var userId = this._adminRep.AddUser(user.Name, user.Username, hashedPassword, restaurantId);
                return Ok(new {
                    userId = userId,
                    password = tempPass
                });
            }
            else
            {
                return StatusCode(409, "The username already exists");
            }
        }

        [HttpDelete]
        [Route("deleteUser/{userid}")]
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteUser(string userid)
        {
            if (_helper.OwnesUser(User, userid, _adminRep, _auth))
            {
                this._adminRep.DeleteUser(userid);
                return Ok();
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("resetPassword/{userid}")]
        [Authorize(Roles ="Manager")]
        public ActionResult ResetPassword(string userid)
        {
            if (_helper.OwnesUser(User, userid, _adminRep, _auth))
            {
                var tempPass = Guid.NewGuid().ToString().Substring(0, 6);
                var user = _adminRep.GetUser(userid);
                user.Password = Crypto.HashPassword(tempPass);
                user.MustChangePassword = true;
                _adminRep.UpdateUser(userid, user);
                return Ok(tempPass);
            }
            return Unauthorized();
        }
    }


    // view models

    public class AddUserVM
    {
        public string Name { get; set; }
        public string Username { get; set; }
    }

    public class UpdatePasswordVM
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
