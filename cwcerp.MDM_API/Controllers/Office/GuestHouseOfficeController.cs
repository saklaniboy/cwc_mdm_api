using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;
using cwcerp.Mdm_Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace cwcerp.MDM_API.Controllers.Office
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestHouseOfficeController : ControllerBase
    {
        private readonly IGuestHouseOfficeService _guestHouseOfficeService;
        public GuestHouseOfficeController(IGuestHouseOfficeService guestHouseOfficeService)
        {
            _guestHouseOfficeService = guestHouseOfficeService;
        }


        [HttpGet]
        [Route("GetGhList")]

        public IActionResult GetGhList(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                Response res = _guestHouseOfficeService.GetGhList(pageNumber, pageSize);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                Response res = new Response
                {
                    success = false,
                    message = "An error occurred while processing the request",
                    responseData = ex.Message
                };
                return StatusCode(500, res);
            }
        }


        [HttpGet]
        [Route("GetGhInfo")]
        public IActionResult GetGhInfo(int officeId)
        {
            try
            {
                Response res = _guestHouseOfficeService.GetGhInfo(officeId);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                Response res = new Response
                {
                    success = false,
                    message = "An error occurred while processing the request",
                    responseData = ex.Message
                };
                return StatusCode(500, res);
            }
        }


        [HttpGet]
        [Route("GetGhDetails")]
        public IActionResult GetGhDetails(int officeId)
        {
            try
            {
                Response res = _guestHouseOfficeService.GetGhDetails(officeId);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                Response res = new Response
                {
                    success = false,
                    message = "An error occurred while processing the request",
                    responseData = ex.Message
                };
                return StatusCode(500, res);
            }
        }


        [HttpPost]
        [Route("BasicInfo")]
        public IActionResult BasicInfo(GhBasicInfo request)
        {
            try
            {
                Response res = _guestHouseOfficeService.BasicInfo(request);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                Response res = new Response
                {
                    success = false,
                    message = "An error occurred while processing the request",
                    responseData = ex.Message
                };
                return StatusCode(500, res);
            }
        }

        [HttpPost]
        [Route("BillingAddressInfo")]
        public IActionResult BillingAddressInfo(GhBillingInfo request)
        {
            try
            {
                Response res = _guestHouseOfficeService.BillingAddressInfo(request);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                Response res = new Response
                {
                    success = false,
                    message = "An error occurred while processing the request",
                    responseData = ex.Message
                };
                return StatusCode(500, res);
            }
        }

        [HttpPost]
        [Route("BankInfo")]
        public IActionResult BankInfo(GhBankInfo request)
        {
            try
            {
                Response res = _guestHouseOfficeService.BankInfo(request);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                Response res = new Response
                {
                    success = false,
                    message = "An error occurred while processing the request",
                    responseData = ex.Message
                };
                return StatusCode(500, res);
            }
        }

        [HttpPost]
        [Route("ContactInfo")]
        public IActionResult ContactInfo(GhContactInfo request)
        {
            try
            {
                Response res = _guestHouseOfficeService.ContactInfo(request);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                Response res = new Response
                {
                    success = false,
                    message = "An error occurred while processing the request",
                    responseData = ex.Message
                };
                return StatusCode(500, res);
            }
        }


        [HttpPost]
        [Route("UpsertOfficeStatus")]
        public IActionResult OfficeStatus(OfficeStatus request)
        {
            try
            {
                Response res = _guestHouseOfficeService.OfficeStatus(request);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                Response res = new Response
                {
                    success = false,
                    message = "An error occurred while processing the request",
                    responseData = ex.Message
                };
                return StatusCode(500, res);
            }
        }

    }
}
