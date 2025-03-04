using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;
using cwcerp.Mdm_Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace cwcerp.MDM_API.Controllers.Office
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionOfficeController : ControllerBase
    {
        private readonly IRegionOfficeService _regionOfficeService;
        public RegionOfficeController(IRegionOfficeService regionOfficeService)
        {
            _regionOfficeService = regionOfficeService;
        }


        [HttpGet]
        [Route("GetRoList")]

        public IActionResult GetRoList(int? pageNumber, int? pageSize, int? statusId)
        {
            try
            {
                Response res = _regionOfficeService.GetRoList(pageNumber, pageSize, statusId);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest(res);
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
        [Route("GetRoInfo")]
        public IActionResult GetRoInfo(int officeId)
        {
            try
            {
                Response res = _regionOfficeService.GetRoInfo(officeId);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest(res);
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
        public IActionResult BasicInfo(RoBasicInfo request)
        {
            try
            {
                Response res = _regionOfficeService.BasicInfo(request);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest(res);
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
        public IActionResult BillingAddressInfo(RoBillingInfo request)
        {
            try
            {
                Response res = _regionOfficeService.BillingAddressInfo(request);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest(res);
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
        public IActionResult BankInfo(RoBankInfo request)
        {
            try
            {
                Response res = _regionOfficeService.BankInfo(request);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest(res);
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
        public IActionResult ContactInfo(RoContactInfo request)
        {
            try
            {
                Response res = _regionOfficeService.ContactInfo(request);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest(res);
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
                Response res = _regionOfficeService.OfficeStatus(request);
                if (res.success)
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest(res);
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
