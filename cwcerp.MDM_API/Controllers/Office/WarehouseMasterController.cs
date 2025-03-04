using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Response;
using cwcerp.Mdm_Service.IService;
using cwcerp.Mdm_Service.Service;
using cwcerp.MDM_Service.IService;
using cwcerp.MDM_Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace cwcerp.MDM_API.Controllers.Office
{

    [Route("api/[controller]")]
    [ApiController]

    public class WarehouseMasterController : ControllerBase
    {

        private readonly IWarehouseMasterService _warehouseMasterService;

        public WarehouseMasterController(IWarehouseMasterService warehouseMasterService)
        {
            _warehouseMasterService = warehouseMasterService;
        }

        [HttpPost]

        public Response AddWarehouseMaster([FromBody] WarehouseMasterModel warehouseMaster)
        {
            Response res = new Response();
            try
            {


                res = _warehouseMasterService.AddWarehouseMaster(warehouseMaster);


            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpPost]
        [Route("AreaDeatails")]

        public Response AddOrUpdateWarehouseAreaDetails([FromBody] WarehouseAreaDetailsModel warehouseMaster)
        {
            Response res = new Response();
            try
            {


                res = _warehouseMasterService.AddOrUpdateWarehouseAreaDetails(warehouseMaster);


            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpPost]
        [Route("AccountDeatails")]

        public Response AddOrUpdateWarehouseAccountDetails([FromBody] WarehouseAccountDetailsModel warehouseMaster)
        {
            Response res = new Response();
            try
            {


                res = _warehouseMasterService.AddOrUpdateWarehouseAccountDetails(warehouseMaster);


            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }


        [HttpPost]
        [Route("BankDeatails")]

        public Response AddWarehouseBankDetails([FromBody] WarehouseBankDetailsModel warehouseMaster)
        {
            Response res = new Response();
            try
            {

                warehouseMaster.CreatedBy = Convert.ToInt32(GetDetailsByToken().UserId);
                res = _warehouseMasterService.AddWarehouseBankDetails(warehouseMaster);


            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpGet]
        [Route("GetWarehouseList")]
        public Response GetWarehouseList([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10,[FromQuery] int? locationId = null) // Added locationId parameter
        {
            Response res = new Response();
            try
            {
                res = _warehouseMasterService.GetWarehouseList(pageNumber, pageSize, locationId);
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

        public Response OfficeStatus([FromBody] cwcerp.MDM_Models.Response.WOfficeStatus warehouseMaster)
        {
            Response res = new Response();
            try
            {


                res = _warehouseMasterService.UpdateOfficeStatus(warehouseMaster);


            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpPost]
        [Route("AdditionalDeatails")]

        public Response AddWarehouseAdditionalDetails([FromBody] WarehouseAdditionalDetailsModel warehouseMaster)
        {
            Response res = new Response();
            try
            {


                res = _warehouseMasterService.AddWarehouseAdditionalDetails(warehouseMaster);


            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpPost("GetWarehouseDetailsById")]
       
        public Response GetWarehouseDetailsById(OfficeCustomModel model)

        {
            Response res = new Response();
            try
            {

                res = _warehouseMasterService.GetWarehouseDetailsById(model.officeId);


            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;

            }
            return res;

        }

        [HttpGet("GetWarehouseCategories")]
        public Response GetWarehouseCategories()
        {
            Response res = new Response();
            try
            {
                var categories =  _warehouseMasterService.GetWarehouseCategories();

                if (categories == null)
                {
                    res.success = false;
                    res.message = "No warehouse categories found.";
                    return res;
                }

                res.success = true;
                res.responseData = categories;
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = "An error occurred.";
               
            }
            return res;
        }

        [HttpGet("GetWarehouseOwner")]
        public Response GetWarehouseOwner()
        {
            Response res = new Response();
            try
            {
                var owner = _warehouseMasterService.GetWarehouseOwner();

                if (owner == null)
                {
                    res.success = false;
                    res.message = "No warehouse owner found.";
                    return res;
                }

                res.success = true;
                res.responseData = owner;
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = "An error occurred.";

            }
            return res;
        }


        [HttpGet("GetWarehouseTypes")]
        public Response GetWarehouseTypes()
        {
            Response res = new Response();
            try
            {
                var types = _warehouseMasterService.GetWarehouseTypes();

                if (types == null)
                {
                    res.success = false;
                    res.message = "No warehouse types found.";
                    return res;
                }

                res.success = true;
                res.responseData = types;
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = "An error occurred.";

            }
            return res;
        }



        private TokenData GetDetailsByToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                var userId = jwtToken?.Claims.First(claim => claim.Type == "UserId").Value;
                return new TokenData()
                {
                    UserId = Convert.ToInt32(userId),
                    CompanyId = 0,
                    ModuleId = 0
                };
            }
            return null;
        }
    }
}
