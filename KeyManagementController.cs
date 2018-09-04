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
    [RoutePrefix("api/keyManagement")]
    public class KeyManagementController : BaseApiController
    {
        IKeyManagement keymgt = new VisaCheckOut.Services.Implementations.KeyManagement();

        [ClaimsAuthorization(ClaimType = "Getapikeys", ClaimValue = "1")]
        [Route("getapikeys")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAPIKeys()
        {

            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.KeyManagementOut.GetClientAPIKeysDTO> res = new Models.DTO<Models.VisaCheckoutOutputs.KeyManagementOut.GetClientAPIKeysDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            try
            {
                res = keymgt.GetAPIKeys();
            }
            catch (Exception ex)
            {


            }
            return Ok(res);

        }

        //[ClaimsAuthorization(ClaimType = "Createapikey", ClaimValue = "1")]
        [Route("createapikey")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateAPIKey(Models.VisaCheckoutInputs.KeyManagementInp.CreateClientAPIKeyinp inp)
        {
            Models.DTO<Models.VisaCheckoutOutputs.KeyManagementOut.ClientAPIKeyDTO> capikeys = new Models.DTO<Models.VisaCheckoutOutputs.KeyManagementOut.ClientAPIKeyDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                capikeys = keymgt.CreateAPIKey(inp);
            }
            catch (Exception ex)
            {


            }
            return Ok(capikeys);

        }

        [ClaimsAuthorization(ClaimType = "Getapikeydetails", ClaimValue = "1")]
        [Route("getapikeydetails")]
        [HttpGet]

        public IHttpActionResult GetAPIKeyDetails([FromUri] string apikey)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.KeyManagementOut.ClientAPIKeyDTO> res = new Models.DTO<Models.VisaCheckoutOutputs.KeyManagementOut.ClientAPIKeyDTO>();

            try
            {
                res = keymgt.GetAPIKeyDetails(apikey);
            }
            catch (Exception ex)
            {


            }
            return Ok(res);

        }

        //[ClaimsAuthorization(ClaimType = "Updateapikey", ClaimValue = "1")]
        [Route("updateapikey")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateAPIKey([FromUri]string apikey, [FromBody]VisaCheckOut.Models.VisaCheckoutInputs.KeyManagementInp.UpdateAPIKeyInp inps)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.KeyManagementOut.UpdateAPIKeyDTO> dto = new Models.DTO<Models.VisaCheckoutOutputs.KeyManagementOut.UpdateAPIKeyDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            try
            {

                dto = keymgt.UpdateAPIKey(apikey, inps);
            }
            catch (Exception ex)
            {

            }
            return Ok(dto);

        }

        [ClaimsAuthorization(ClaimType = "Deleteapikey", ClaimValue = "1")]
        [Route("deleteapikey")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAPIKey([FromUri]string apikey)
        {
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            string status = "";
            try
            {
                status = keymgt.DeleteAPIKey(apikey);
            }
            catch (Exception ex)
            {

            }
            return Ok(status);

        }

    }
}
