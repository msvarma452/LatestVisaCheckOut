using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisaCheckOut.Infrastructure;
using VisaCheckOut.Services;
using VisaCheckOut.Services.Interfaces;

namespace VisaCheckOut.Controllers
{
    [RoutePrefix("api/profile")]
    public class ProfileManagementController : BaseApiController
    {
        IProfileManagement profilemgt = new VisaCheckOut.Services.Implementations.ProfileManagement();

       // [ClaimsAuthorization(ClaimType = "Getallprofiles", ClaimValue = "1")]
        [Route("getallprofiles")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllProfiles()
        {
           Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.ProfileManagementOut.GetAllProfilesDTO> res = new Models.DTO<Models.VisaCheckoutOutputs.ProfileManagementOut.GetAllProfilesDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            try
            {
                res = profilemgt.GetAllProfiles();
            }
            catch (Exception ex)
            {


            }
            return Ok(res);
        }

        //[ClaimsAuthorization(ClaimType = "Getallprofiles", ClaimValue = "1")]
        [Route("getallprofileslist")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllProfileslist()
        {
            List<VisaCheckOut.Models.VisaCheckoutOutputs.ProfileManagementOut.ProfileListDTO> res = new List<Models.VisaCheckoutOutputs.ProfileManagementOut.ProfileListDTO>();           
            try
            {
                res = profilemgt.GetAllProfileslist();
            }
            catch (Exception ex)
            {


            }
            return Ok(res);
        }


        [ClaimsAuthorization(ClaimType = "Createcheckoutprofile", ClaimValue = "1")]
        [Route("createcheckoutprofile")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateCheckoutProfile(VisaCheckOut.Models.VisaCheckoutInputs.ProfileManagementInp.CreateCheckoutProfileInp inp)
        {
           Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.ProfileManagementOut.CreateCheckoutProfileDTO> res = new Models.DTO<Models.VisaCheckoutOutputs.ProfileManagementOut.CreateCheckoutProfileDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                res = profilemgt.CreateCheckoutProfile(inp);
            }
            catch (Exception ex)
            {


            }
            return Ok(res);
        }

        //[ClaimsAuthorization(ClaimType = "GetSpecificCheckoutProfile", ClaimValue = "1")]
        [Route("getSpecificCheckoutProfile")]
        [HttpGet]
        public IHttpActionResult GetSpecificCheckoutProfile([FromUri]string externalProfileID)
        {
           Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.ProfileManagementOut.GetSpecificCheckoutProfileDTO> res = new Models.DTO<Models.VisaCheckoutOutputs.ProfileManagementOut.GetSpecificCheckoutProfileDTO>();
            try
            {
                res = profilemgt.GetSpecificCheckoutProfile(externalProfileID);
            }
            catch (Exception ex)
            {


            }
            return Ok(res);
        }

        [ClaimsAuthorization(ClaimType = "UpdateSpecificCheckoutProfile", ClaimValue = "1")]
        [Route("updateSpecificCheckoutProfile")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateSpecificCheckoutProfile([FromUri]string ExternalProfileID ,VisaCheckOut.Models.VisaCheckoutInputs.ProfileManagementInp.UpdateSpecificCheckoutProfileInp inp)
        {
           Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.ProfileManagementOut.GetSpecificCheckoutProfileDTO> res = new Models.DTO<Models.VisaCheckoutOutputs.ProfileManagementOut.GetSpecificCheckoutProfileDTO>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                res = profilemgt.UpdateSpecificCheckoutProfile(ExternalProfileID,inp);
            }
            catch (Exception ex)
            {


            }
            return Ok(res);
        }

        [ClaimsAuthorization(ClaimType = "DeleteCheckoutProfile", ClaimValue = "1")]
        [Route("deleteCheckoutProfile")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCheckoutProfile([FromUri] string externalProfileId)
        {
            Models.DTO<string> dto = new Models.DTO<string>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            try
            {
                dto = profilemgt.DeleteCheckoutProfile(externalProfileId);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }




    }
}
