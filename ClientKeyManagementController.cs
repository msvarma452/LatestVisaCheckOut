using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisaCheckOut.Infrastructure;
using VisaCheckOut.Services.Interfaces;

namespace VisaCheckOut.Controllers
{
    [RoutePrefix("api/clientkeyManagement")]
    public class ClientKeyManagementController : BaseApiController
    {
        IKeyManagement keymgt = new VisaCheckOut.Services.Implementations.KeyManagement();
        //[ClaimsAuthorization(ClaimType = "Getclientapikeys", ClaimValue = "1")]
        [Route("getclientapikeys")]
        [HttpGet]
        public async Task<IHttpActionResult> GetClientAPIKeys([FromUri]string externalClientId)
        {
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.KeyManagementOut.GetClientAPIKeysDTO> res = new Models.DTO<Models.VisaCheckoutOutputs.KeyManagementOut.GetClientAPIKeysDTO>();
            try
            {
                res = keymgt.GetClientAPIKeys(externalClientId);
            }
            catch (Exception ex)
            {


            }
            return Ok(res);

        }

        [ClaimsAuthorization(ClaimType = "Createclientapikey", ClaimValue = "1")]
        [Route("createclientapikey")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateClientAPIKey([FromUri]string externalClientId)
        {
            Models.DTO<Models.VisaCheckoutOutputs.KeyManagementOut.ClientAPIKeyDTO> capikeys = new Models.DTO<Models.VisaCheckoutOutputs.KeyManagementOut.ClientAPIKeyDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            try
            {
                capikeys = keymgt.CreateClientAPIKey(externalClientId);
            }
            catch (Exception ex)
            {


            }
            return Ok(capikeys);

        }

        [ClaimsAuthorization(ClaimType = "Getclientapikeydetails", ClaimValue = "1")]
        [Route("getclientapikeydetails")]
        [HttpGet]

        public async Task<IHttpActionResult> GetClientAPIKeyDetails([FromUri]string externalClientId)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.KeyManagementOut.ClientAPIKeyDTO> res = new Models.DTO<Models.VisaCheckoutOutputs.KeyManagementOut.ClientAPIKeyDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            try
            {
                res = keymgt.GetClientAPIKeyDetails(externalClientId);
            }
            catch (Exception ex)
            {


            }
            return Ok(res);

        }

        //[ClaimsAuthorization(ClaimType = "Updateclientapikey", ClaimValue = "1")]
        [Route("updateclientapikey")]
        [HttpPost]
        public IHttpActionResult UpdateAPIKey([FromUri]string externalClientId, [FromUri] string key, [FromBody]VisaCheckOut.Models.VisaCheckoutInputs.KeyManagementInp.UpdateAPIKeyInp inp)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.KeyManagementOut.UpdateAPIKeyDTO> dto = new Models.DTO<Models.VisaCheckoutOutputs.KeyManagementOut.UpdateAPIKeyDTO>();

            try
            {

                dto = keymgt.UpdateClientAPIKey(externalClientId, key, inp);
            }
            catch (Exception ex)
            {

            }
            return Ok(dto);

        }

        [ClaimsAuthorization(ClaimType = "Deleteclientapikey", ClaimValue = "1")]
        [Route("deleteclientapikey")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteClientAPIKey([FromUri]string externalClientId,[FromUri]string apiKeyValue)
        {
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            string status = "";
            try
            {
                status = keymgt.DeleteClientAPIKey(externalClientId,apiKeyValue);
            }
            catch (Exception ex)
            {

            }
            return Ok(status);

        }
                 
    }
}
