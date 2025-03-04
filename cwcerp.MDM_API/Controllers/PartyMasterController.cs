
using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Response;
using cwcerp.MDM_Service.IService;
using cwcerp.MDM_Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ValidationHelper;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using cwcerp.MDM_Models.Request;
namespace cwcerp.MDM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartyMasterController : ControllerBase
    {
        private readonly IPartyMasterService _partyMasterService;
        public PartyMasterController(IPartyMasterService partyMasterService)
        {
            _partyMasterService = partyMasterService;
        }
        [HttpPost]
        public IActionResult AddPartyMaster([FromBody] List<PartyMasterModel> partyMasterList)
        {
            var response = new Response();

            // Manually validate the models
            var validationErrors = new List<ValidationError>();

            // Validate each partyMaster object in the list
            foreach (var partyMaster in partyMasterList)
            {
                validationErrors.AddRange(ValidationHelper.ValidateModel(partyMaster));
            }

            // If there are validation errors, group them by code
            if (validationErrors.Any())
            {
                var groupedErrors = validationErrors
                    .GroupBy(e => e.Code)
                    .Select(group => new ValidationError
                    {
                        Code = group.Key,
                        FieldName = string.Join(", ", group.Select(e => e.FieldName)),
                        Message = group.First().Message // Assuming all messages for the same code are identical
                    })
                    .FirstOrDefault();

                response.success = false;
                response.message = $"{groupedErrors?.Message}: {groupedErrors?.FieldName}";
                response.errorCode = groupedErrors?.Code; // General validation error code
                return BadRequest(response);
            }

            try
            {
                // Business logic for adding PartyMaster(s)
                var result = _partyMasterService.AddPartyMaster(partyMasterList);

                if (!result.success)
                {
                    response.success = false;
                    response.message = result.message;
                    response.errorCode = 1003; // Specific error code for service failure
                    return Ok(response);
                }
                else
                {
                    response.success = true;
                    response.message = "Party master(s) added successfully.";
                    return Ok(response);
                }
                
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An unexpected error occurred.";
                response.errorCode = 5000; // Internal server error
                return StatusCode(500, response);
            }
        }




        [HttpGet]
        [Route("GetPartyList")]
        public Response GetPartyList([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            Response res = new Response();
            try
            {
                res = _partyMasterService.GetPartyList(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetDepositorList")]
        public Response GetPartyList(int DepositorMainType = 0, int DipositorSubType = 0, string TypeOfServices = "", string searchText = "")
        {
            Response res = new Response();
            try
            {
                res = _partyMasterService.GetDipositorList(DepositorMainType, DipositorSubType, TypeOfServices);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }


        [HttpPost]
        [Route("UpdatePartyStatus")]
        public Response UpdatePartyStatus([FromBody] PartyStatusModel model)
        {
            Response response = new Response();
            try
            {
                response = _partyMasterService.UpdatePartyStatus(model);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("GetDepositorListBySearchText/{searchText}")]
        public Response GetDepositorListBySearchText(string searchText)
        {
            Response res = new Response();
            try
            {
                res = _partyMasterService.GetDipositorList(searchText: searchText);
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