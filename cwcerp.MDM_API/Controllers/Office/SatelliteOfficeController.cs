using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;
using cwcerp.Mdm_Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace cwcerp.MDM_API.Controllers.Office
{
    [Route("api/[controller]")]
    [ApiController]
    public class SatelliteOfficeController : ControllerBase
    {
        private readonly ISatelliteOfficeService _satelliteOfficeService;
        public SatelliteOfficeController(ISatelliteOfficeService satelliteOfficeService)
        {
            _satelliteOfficeService = satelliteOfficeService;
        }


        [HttpGet]
        [Route("GetSoList")]

        public IActionResult GetSoList(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                Response res = _satelliteOfficeService.GetSoList(pageNumber, pageSize);
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
        [Route("GetSoInfo")]
        public IActionResult GetSoInfo(int officeId)
        {
            try
            {
                Response res = _satelliteOfficeService.GetSoInfo(officeId);
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
        [Route("GetSoDetails")]
        public IActionResult GetSoDetails(int officeId)
        {
            try
            {
                Response res = _satelliteOfficeService.GetSoDetails(officeId);
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
        public IActionResult BasicInfo(SoBasicInfo request)
        {
            try
            {
                Response res = _satelliteOfficeService.BasicInfo(request);
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
        public IActionResult BillingAddressInfo(SoBillingInfo request)
        {
            try
            {
                Response res = _satelliteOfficeService.BillingAddressInfo(request);
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
        public IActionResult BankInfo(SoBankInfo request)
        {
            try
            {
                Response res = _satelliteOfficeService.BankInfo(request);
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
        public IActionResult ContactInfo(SoContactInfo request)
        {
            try
            {
                Response res = _satelliteOfficeService.ContactInfo(request);
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
                Response res = _satelliteOfficeService.OfficeStatus(request);
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
