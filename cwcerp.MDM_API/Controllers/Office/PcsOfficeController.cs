using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Response;
using cwcerp.Mdm_Service.IService;
using cwcerp.Mdm_Service.Service;
using cwcerp.MDM_Service.IService;
using cwcerp.MDM_Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace cwcerp.MDM_API.Controllers.Office
{

    [Route("api/[controller]")]
    [ApiController]

    public class PcsOfficeController : ControllerBase
    {

        private readonly IPcsOfficeService _pcsOfficeService;

        public PcsOfficeController(IPcsOfficeService pcsOfficeService)
        {
            _pcsOfficeService = pcsOfficeService;
        }

        [HttpPost]
        [Route("InsertPCSBasicInfo")]
        public Response AddPcsBasicInfo([FromBody] PcsBasicInfoModel pcsOffice)
        {
            Response res = new Response();
            try
            {
                res = _pcsOfficeService.AddPcsBasicInfo(pcsOffice);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpPost]
        [Route("UpsertPCSAccountInfo")]

        public Response AddOrUpdatePcsAccountDetails([FromBody] PcsAccountDetails pcsOffice)
        {
            Response res = new Response();
            try
            {
                res = _pcsOfficeService.AddOrUpdatePcsAccountDetails(pcsOffice);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpPost]
        [Route("UpsertPCSBankInfo")]

        public Response AddPcsBankDetails([FromBody] PcsBankDetailsModel pcsOffice)
        
        {
            Response res = new Response();
            try
            {
                res = _pcsOfficeService.AddPcsBankDetails(pcsOffice);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpPost]
        [Route("UpsertPCSAdditionalInfo")]

        public Response AddPcsAdditionalDetails([FromBody] PcsAdditionalDetailsModel pcsOffice)
        {
            Response res = new Response();
            try
            {
                res = _pcsOfficeService.AddPcsAdditionalDetails(pcsOffice);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpGet]
        [Route("GetPcsList")]
        public Response GetPcsList([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            Response res = new Response();
            try
            {
                res = _pcsOfficeService.GetPcsList(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }


        [HttpPost]
        [Route("OfficeStatus")]

        public Response OfficeStatus([FromBody] cwcerp.MDM_Models.Response.WOfficeStatus pcsOffice)
        {
            Response res = new Response();
            try
            {
                res = _pcsOfficeService.UpdateOfficeStatus(pcsOffice);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpGet("GetPcsDetailsById/{officeId}")]

        public Response GetPcsDetailsById(int officeId)
        {
            Response res = new Response();
            try
            {

                res = _pcsOfficeService.GetPcsDetailsById(officeId);


            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }
    }
}
