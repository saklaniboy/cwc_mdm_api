using cwcerp.Mdm_Models.Response;
using cwcerp.Mdm_Service.IService;
using cwcerp.Mdm_Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace cwcerp.MDM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;
        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }


        [HttpGet]
        [Route("GetAuditLogs")]

        public IActionResult GetAuditLogs()
        {
            try
            {
                Response res = _auditLogService.GetAuditLogs();              
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
