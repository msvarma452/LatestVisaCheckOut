using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisaCheckOut.Infrastructure;
using VisaCheckOut.Models.VisaCheckoutOutputs.ClientUserManagementOut;

namespace VisaCheckOut.Controllers
{
    [RoutePrefix("api/clientusermanagement")]
    public class ClientUserManagementController : BaseApiController
    {

        VisaCheckOut.Services.Interfaces.IClientUserManagement cum = new Services.Implementations.ClientUserManagement();

        [ClaimsAuthorization(ClaimType = "GetClientUsers", ClaimValue = "1")]
        [Route("getClientUsers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetClientUsers([FromUri]string externalClientId)
        {
            Models.DTO<GetClientUsersDTO> dto = new Models.DTO<GetClientUsersDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            try
            {
                dto = cum.GetClientUsers(externalClientId);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }

        //  CreateUserForClient
        [ClaimsAuthorization(ClaimType = "CreateUserForClient", ClaimValue = "1")]
        [Route("createUserForClient")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateUserForClient([FromUri]string externalClientId,VisaCheckOut.Models.VisaCheckoutInputs.ClientUserManagementInp.CreateUserForClientInp inp)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.ClientUserManagementOut.CreateUserForClientDTO> dto = new Models.DTO<CreateUserForClientDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                dto = cum.CreateUserForClient(externalClientId,inp);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }


        // UpdateUserForClient
        [ClaimsAuthorization(ClaimType = "UpdateUserForClient", ClaimValue = "1")]
        [Route("updateUserForClient")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateUserForClient([FromUri]string externalClientId,[FromUri]string username,VisaCheckOut.Models.VisaCheckoutInputs.ClientUserManagementInp.UpdateUserForClientInp inp)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.ClientUserManagementOut.UpdateUserForClientDTO> dto = new Models.DTO<UpdateUserForClientDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                dto = cum.UpdateUserForClient(externalClientId,username,inp);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }


        // DeleteUserDetailsForClient
        //[ClaimsAuthorization(ClaimType = "DeleteUserDetailsForClient", ClaimValue = "1")]
        [Route("deleteUserDetailsForClient")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUserDetailsForClient([FromUri]string externalClientId,[FromUri]string username)
        {
            string status = "";
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            try
            {
                status = cum.DeleteUserDetailsForClient(externalClientId,username);
            }
            catch (Exception ex)
            {


            }
            return Ok(status);
        }
    }
}
