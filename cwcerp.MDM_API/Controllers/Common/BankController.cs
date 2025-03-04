using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;
using cwcerp.Mdm_Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace cwcerp.MDM_API.Controllers.Office
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }


        [HttpGet]
        [Route("GetBankList")]

        public IActionResult GetBankList()
        {
            try
            {
                Response res = _bankService.GetBankList();
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
        [Route("GetBankIFSC")]

        public IActionResult GetBankIFSC(int bankId)
        {
            try
            {
                Response res = _bankService.GetBankIFSC(bankId);
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
