using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VisaCheckOut.Infrastructure;
using VisaCheckOut.Models.VisaCheckoutOutputs.RelationshipManagementOut;
using VisaCheckOut.Services.Interfaces;

namespace VisaCheckOut.Controllers
{
    [RoutePrefix("api/relationshipManagement")]
    public class RelationshipManagementController : BaseApiController
    {
        IRelationshipManagement relarionship = new VisaCheckOut.Services.Implementations.RelationshipManagement();

        [ClaimsAuthorization(ClaimType = "Getallmerchants", ClaimValue = "1")]
        [Route("Getallmerchants")]
        [HttpGet]
        public async Task<IHttpActionResult> Getallmerchants()
        {
           List<VisaCheckOut.Models.VisaCheckoutOutputs.RelationshipManagementOut.ClientOnboardingDTO> dto = new List<Models.VisaCheckoutOutputs.RelationshipManagementOut.ClientOnboardingDTO>();
            try
            {
                VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
                string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
                var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
                inp.UserId = useriD;
                inp.RoleName = currentRoles;
                dto = relarionship.Getallmerchants(inp);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);

        }

        //[ClaimsAuthorization(ClaimType = "Getallmerchantsdropdown", ClaimValue = "1")]
        [Route("Getallmerchantsdropdown")]
        [HttpGet]
        public async Task<IHttpActionResult> Getallmerchantsdropdown()
        {
            List<VisaCheckOut.Models.VisaCheckoutOutputs.RelationshipManagementOut.GetMerchantsDrpdnDTO> dto = new List<Models.VisaCheckoutOutputs.RelationshipManagementOut.GetMerchantsDrpdnDTO>();
            try
            {
                VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
               // string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
                //var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
                //inp.UserId = useriD;
                //inp.RoleName = currentRoles;
                dto = relarionship.Getallmerchantsdropdown(inp);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);

        }


        //Client Onboarding
        [ClaimsAuthorization(ClaimType = "CreateMerchant", ClaimValue = "1")]
        [Route("CreateMerchant")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateNewClient(VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.ClientOnboardingInp inp)
        {
            VisaCheckOut.Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.RelationshipManagementOut.ClientOnboardingDTO> dto = new Models.DTO<ClientOnboardingDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                Errordetail err = new Errordetail();              
                dto = relarionship.CreateNewClient(inp);
                if(dto.response.message=="Created")
                {                    
                    return Ok(dto);
                }
            }
            catch (Exception ex)
            {
                return Ok(dto);
            }
            return Ok(dto);
        }

       // [ClaimsAuthorization(ClaimType = "GetRelationships", ClaimValue = "1")]
        [Route("getRelationships")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRelationships([FromUri]string externalClientId)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.RelationshipManagementOut.GetRelationshipsDTO> dto = new Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.RelationshipManagementOut.GetRelationshipsDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                dto = relarionship.GetRelationships(externalClientId);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }
        //[ClaimsAuthorization(ClaimType = "UpdateRelationship", ClaimValue = "1")]
        [Route("updateRelationship")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRelationship([FromUri]string externalclientId, VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.Update_RelationshipInp inp)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.RelationshipManagementOut.GetRelationshipsDTO> dto = new Models.DTO<GetRelationshipsDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                dto = relarionship.UpdateRelationship(externalclientId, inp);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }


        [ClaimsAuthorization(ClaimType = "DeleteRelationship", ClaimValue = "1")]
        [Route("delateRelationship")]
        [HttpDelete]
        public async Task<IHttpActionResult> DelateRelationship([FromUri]string externalClientId)
        {
            string res = "";
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                res = relarionship.DeleteRelationship(externalClientId);
            }
            catch (Exception ex)
            {


            }
            return Ok(res);
        }

        [ClaimsAuthorization(ClaimType = "GetClientDetails", ClaimValue = "1")]
        [Route("getClientDetails")]
        [HttpGet]
        public async Task<IHttpActionResult> GetClientDetails([FromUri]string externalClientId)
        {
            VisaCheckOut.Models.VisaCheckoutOutputs.RelationshipManagementOut.GetClientDetailsDTO resp = new VisaCheckOut.Models.VisaCheckoutOutputs.RelationshipManagementOut.GetClientDetailsDTO();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                resp = relarionship.GetClientDetails(externalClientId);
            }
            catch (Exception ex)
            {


            }
            return Ok(resp);
        }

        [ClaimsAuthorization(ClaimType = "UpdateClientDetails", ClaimValue = "1")]
        [Route("updateClientDetails")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateClientDetails([FromUri]string externalClientId, VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UpdateClientDetailsInp inp)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.RelationshipManagementOut.UpdateClientDetailsDTO> dto = new Models.DTO<UpdateClientDetailsDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                dto = relarionship.UpdateClientDetails(externalClientId, inp);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }



    }
}
