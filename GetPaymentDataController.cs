using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisaCheckOut.Infrastructure;
using VisaCheckOut.Services.Implementations;
using VisaCheckOut.Services.Interfaces;

namespace VisaCheckOut.Controllers
{
    [RoutePrefix("api/getpaymentdata")]
    public class GetPaymentDataController : BaseApiController
    {
        IGetPaymentData paymentdata = new GetPaymentData();
        //[ClaimsAuthorization(ClaimType = "GetpaymentdataInfo", ClaimValue = "1")]
        [Route("getpaymentdataInfo")]
        [HttpGet]
        public async Task<IHttpActionResult> GetpaymentdataInfo([FromUri]string Callid, [FromUri]string OrderId, [FromUri]string Merchantid)
        {
           VisaCheckOut.Models.VisaCheckoutOutputs.GetPaymentDataOut.PaymentStatusDTO dto = new Models.VisaCheckoutOutputs.GetPaymentDataOut.PaymentStatusDTO();
            //VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            //string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            //inp.UserId = useriD;
            //inp.RoleName = currentRoles;
            try
            {
                dto = paymentdata.GetPaymentInfo(Callid, OrderId, Merchantid);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }

        [ClaimsAuthorization(ClaimType = "PaymentdataInfo", ClaimValue = "1")]
        [Route("paymentdataInfo")]
        [HttpGet]
        public IHttpActionResult PaymentdataInfo([FromUri]string Callid)
        {
            Models.DTO<List<VisaCheckOut.Models.VisaCheckoutOutputs.GetPaymentDataOut.PaymentDataDTO>> dto = new Models.DTO<List<Models.VisaCheckoutOutputs.GetPaymentDataOut.PaymentDataDTO>>();
            try
            {
                dto = paymentdata.PaymentInfo(Callid);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }

        [ClaimsAuthorization(ClaimType = "Gettransactiondata", ClaimValue = "1")]
        [Route("gettransactiondata")]
        [HttpGet]
        public async Task<IHttpActionResult> Gettransactiondata()
        {
            Models.DTO<List<VisaCheckOut.Models.VisaCheckoutOutputs.GetPaymentDataOut.TransactionDTO>> dto = new Models.DTO<List<Models.VisaCheckoutOutputs.GetPaymentDataOut.TransactionDTO>>();
            VisaCheckOut.Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput inp = new Models.VisaCheckoutInputs.RelationshipManagementInp.UserInput();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentRoles = await this.AppUserManager.GetRolesAsync(useriD);
            inp.UserId = useriD;
            inp.RoleName = currentRoles;
            try
            {
                dto = paymentdata.Gettransactiondata(inp);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }
    }
}
