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

    [RoutePrefix("api/clientprofile")]
    public class ClientProfileManagementController : BaseApiController
    {
        Services.Interfaces.IClientProfileManagement usermgt = new VisaCheckOut.Services.Implementations.ClientProfileManagement();

        [ClaimsAuthorization(ClaimType = "GetClientProfiles", ClaimValue = "1")]
        [Route("getClientProfiles")]
        [HttpGet]
        public async Task<IHttpActionResult> GetClientProfiles([FromUri]string externalClientId)
        {
            VisaCheckOut.Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.ClientProfileManagementOut.GetClientProfilesDTO> dto = new Models.DTO<Models.VisaCheckoutOutputs.ClientProfileManagementOut.GetClientProfilesDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            try
            {
                dto = usermgt.GetClientProfiles(externalClientId);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto.response.profiles.OrderByDescending(a=>a.created));
        }

        [ClaimsAuthorization(ClaimType = "CreateProfile", ClaimValue = "1")]
        [Route("createProfile")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateProfile([FromUri]string externalClientId,VisaCheckOut.Models.VisaCheckoutInputs.ClientProfileManagementInp.CreateProfileInp inp)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.ClientProfileManagementOut.CreateProfileDTO> dto = new Models.DTO<Models.VisaCheckoutOutputs.ClientProfileManagementOut.CreateProfileDTO>();          
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                if (inp != null || (ModelState.IsValid))
                {
                    dto = usermgt.CreateProfile(externalClientId,inp);
                }
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }

        [ClaimsAuthorization(ClaimType = "GetSpecificClientProfile", ClaimValue = "1")]
        [Route("getSpecificClientProfile")]
        [HttpGet]
        public IHttpActionResult GetSpecificClientProfile([FromUri]string externalClientId, [FromUri]string externalProfileId)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.ClientProfileManagementOut.GetSpecificClientProfileDTO> dto = new Models.DTO<Models.VisaCheckoutOutputs.ClientProfileManagementOut.GetSpecificClientProfileDTO>();

            try
            {
                dto = usermgt.GetSpecificClientProfile(externalClientId, externalProfileId);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }
        //[ClaimsAuthorization(ClaimType = "UpdateClientProfile", ClaimValue = "1")]
        [Route("updateClientProfile")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateClientProfile([FromUri]string externalClientId,[FromUri]string externalProfileId, VisaCheckOut.Models.VisaCheckoutInputs.ClientProfileManagementInp.UpdateClientProfileInp inp)
        {

            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.ClientProfileManagementOut.UpdateClientProfileDTO> dto = new Models.DTO<Models.VisaCheckoutOutputs.ClientProfileManagementOut.UpdateClientProfileDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                if (inp != null)
                {
                    dto = usermgt.UpdateClientProfile(externalClientId,externalProfileId, inp);
                }

            }
            catch (Exception ex)
            {


            }
            return Ok(dto);

        }

        //[ClaimsAuthorization(ClaimType = "DeleteClientProfile", ClaimValue = "1")]
        [Route("deleteClientProfile")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteClientProfile([FromUri]string externalClientId, [FromUri]string externalProfileId)
        {
            string status = "";
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                status = usermgt.DeleteClientProfile(externalClientId, externalProfileId);
            }
            catch (Exception ex)
            {


            }
            return Ok(status);
        }

    }
}
