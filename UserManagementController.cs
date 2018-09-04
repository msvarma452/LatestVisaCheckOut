using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisaCheckOut.Infrastructure;

namespace VisaCheckOut.Controllers
{
    [RoutePrefix("api/userManagement")]
    public class UserManagementController : BaseApiController
    {
        Services.Interfaces.IUserManagement usermgt = new VisaCheckOut.Services.Implementations.UserManagement();


        //[ClaimsAuthorization(ClaimType = "GetAllUsers", ClaimValue = "1")]
        [Route("getAllUsers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllUsers()
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.UserManagementOut.GetAllUsersDTO> res = new Models.DTO<Models.VisaCheckoutOutputs.UserManagementOut.GetAllUsersDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                res = usermgt.GetAllUsers();
            }
            catch (Exception ex)
            {


            }
            return Ok(res);
        }


        [ClaimsAuthorization(ClaimType = "Createuser", ClaimValue = "1")]
        [Route("createUser")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser(VisaCheckOut.Models.VisaCheckoutInputs.UserManagementInp.CreateUserInp inp)
        {
           Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.UserManagementOut.CreateUserDTO> res = new Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.UserManagementOut.CreateUserDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                res = usermgt.CreateUser(inp);
            }
            catch (Exception ex)
            {

            }
            return Ok(res);
        }

        [ClaimsAuthorization(ClaimType = "UpdateUser", ClaimValue = "1")]
        [Route("updateUser")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateUser([FromUri]string username, VisaCheckOut.Models.VisaCheckoutInputs.UserManagementInp.CreateUserInp inp)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.UserManagementOut.CreateUserDTO> res = new Models.DTO<Models.VisaCheckoutOutputs.UserManagementOut.CreateUserDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                res = usermgt.UpdateUser(username,inp);
            }
            catch (Exception ex)
            {

            }
            return Ok(res);
        }

        [ClaimsAuthorization(ClaimType = "DeleteUser", ClaimValue = "1")]
        [Route("deleteUser")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser([FromUri]string username)
        {
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            string status= "";
            try
            {
                status= usermgt.DeleteUser(username);
            }
            catch (Exception ex)
            {


            }
            return Ok(status);
        }

    }
}
