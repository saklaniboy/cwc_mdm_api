using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;
using cwcerp.Mdm_Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace cwcerp.MDM_API.Controllers.Office
{
    [Route("api/[controller]")]
    [ApiController]
    public class GstController : ControllerBase
    {
        //Dummy data for GST details
        private static readonly List<GstDetails> DummyGstData = new List<GstDetails>
        {
            new GstDetails
            {
                GstNumber = "27ABCDE1234F1Z5",
                BusinessName = "Regional Office Panchkula",
                PanNumber = "ABCDE1235F",
                TanNumber = "ABCDE1234F",
                AddressLine1 = "123 Main St",
                StateId = "10",
                DistrictId = "112",
                CityId = "30",
                PinCode = "123456",                
            },
            new GstDetails
            {
                GstNumber = "27ABCDE1234F1Z6",
                BusinessName = "Regional Office Chandigarh",
                PanNumber = "ABCDE1236F",
                TanNumber = "ABCDE1237F",
                AddressLine1 = "321 Main St",
                StateId = "11",
                DistrictId = "113",
                CityId = "31",
                PinCode = "123456",
            }
        };

        [HttpGet]
        [Route("gstInfo")]

        public IActionResult GetGstDetails(string gstNumber)
        {
            try
            {
                var gstDetails = DummyGstData.FirstOrDefault(x => x.GstNumber == gstNumber);

                if (gstDetails == null)
                {
                    return NotFound(new { message = "GST details not found." });
                }

                return Ok(gstDetails);
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
