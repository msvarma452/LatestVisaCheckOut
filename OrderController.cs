using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VisaCheckOut.Infrastructure;

namespace VisaCheckOut.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrderController : BaseApiController
    {
        VisaCheckOut.Services.Interfaces.IOrder ord = new VisaCheckOut.Services.Implementations.Order();
        [ClaimsAuthorization(ClaimType = "submitorders", ClaimValue ="1")]
        [Route("submitorders")]
        [HttpPost]
        public IHttpActionResult Submitorder(VisaCheckOut.Models.VisaCheckoutInputs.OrdersInp.OrderInps obj)
        {
            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.OrderOut.OrderDTO> dto = new Models.DTO<Models.VisaCheckoutOutputs.OrderOut.OrderDTO>();
            try
            {
                dto = ord.Submitorder(obj);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }

        [Route("getorders")]
        [HttpPut]
        public IHttpActionResult Getorders(VisaCheckOut.Models.VisaCheckoutInputs.OrdersInp.GetordersInp obj)
        {

            Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.OrderOut.OrdersDTO> dto = new Models.DTO<Models.VisaCheckoutOutputs.OrderOut.OrdersDTO>();
            try
            {
                dto = ord.Getorders(obj);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }
        [Route("getordersbyid")]
        [HttpGet]
        public IHttpActionResult GetordersByID(string OrderId)
        {

          VisaCheckOut.Models.VisaCheckoutOutputs.OrderOut.Orders dto = new Models.VisaCheckoutOutputs.OrderOut.Orders();
            try
            {
                dto = ord.GetordersByID(OrderId);
            }
            catch (Exception ex)
            {


            }
            return Ok(dto);
        }
    }
}
