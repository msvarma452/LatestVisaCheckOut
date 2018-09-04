using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisaCheckOut.Infrastructure;
using VisaCheckOut.Models.VisaCheckoutInputs.UpdatePaymentInformationInp;
using VisaCheckOut.Services.Interfaces;

namespace VisaCheckOut.Controllers
{
    [RoutePrefix("api/updatepayment")]
    public class UpdatePaymentInformationController : BaseApiController
    {
        IUpdatePaymentInformation updatepaymentinfo = new VisaCheckOut.Services.Implementations.UpdatePaymentInformation();

        [ClaimsAuthorization(ClaimType = "Updatepaymentinfo", ClaimValue = "1")]
        [Route("updatepaymentinfo")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdatePaymentInfo([FromUri]string Callid, [FromBody] UpdatePaymentInformationInp inp)
        {
            string status = "";
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inps = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inps.UserId = useriD;
            inps.RoleName = currentRoles;
            try
            {
                status = updatepaymentinfo.UpdatePaymentInfo(inp, Callid);
                if (status!=null)
                {
                    status = "OK";
                }
            }
            catch (Exception ex)
            {

            }
            return Ok(status);
        }

        //[ClaimsAuthorization(ClaimType = "Updatepaymendata", ClaimValue = "1")]
        [Route("Updatepaymendata")]
        [HttpPut]
        public async Task<IHttpActionResult> Updatepaymendata([FromBody] UpdatePaymentInformationInp inp,[FromUri]string merchantid)
        {
            string status = "";
            try
            {
                status = updatepaymentinfo.UpdatePaymentdata(inp, merchantid);
                if (status != null)
                {
                    status = "OK";
                }
            }
            catch (Exception ex)
            {

            }
            return Ok(status);
        }
    }
}
