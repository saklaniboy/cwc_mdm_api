using cwcerp.Mdm_Models.Response;
using Microsoft.AspNetCore.Mvc;
using static ValidationHelper;
using Microsoft.AspNetCore.Authorization;
using cwcerp.MDM_Service.IService;
using System.Text;


namespace cwcerp.MDM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorMasterController : ControllerBase
    {
        private readonly IVendorMasterService _vendorMasterService;
        private object insertedVendors;
        private readonly IAuthService _authService;

        public VendorMasterController(IVendorMasterService vendorMasterService, IAuthService authService)
        {
            _vendorMasterService = vendorMasterService;
            _authService = authService;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("UpsertVendorMaster")]
        public IActionResult UpsertVendorMaster([FromBody] List<VendorMasterModel> vendorMasterList, [FromHeader] string authorization)
        {
            // Validate Authorization Header
            if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Basic "))
            {
                return Unauthorized(new { success = false, message = "Authorization header is missing or invalid." });
            }

            // Decode Basic Auth Credentials
            try
            {
                var encodedCredentials = authorization.Substring("Basic ".Length).Trim();
                var decodedBytes = Convert.FromBase64String(encodedCredentials);
                var decodedString = Encoding.UTF8.GetString(decodedBytes);

                var credentials = decodedString.Split(':');
                if (credentials.Length != 2)
                {
                    return Unauthorized(new { success = false, message = "Invalid Basic Auth format." });
                }

                string username = credentials[0];
                string password = credentials[1];

                // Validate credentials dynamically
                if (!_authService.ValidateCredentials(username, password))
                {
                    return Unauthorized(new { success = false, message = "Invalid username or password." });
                }
            }
            catch
            {
                return Unauthorized(new { success = false, message = "Failed to decode authentication header." });
            }

            if (vendorMasterList == null || !vendorMasterList.Any())
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid input: vendorMasterList is null or empty.",
                    data = new
                    {
                        successEntries = new List<object>(),
                        errorEntries = new List<object>()
                    }
                });
            }

            var successEntries = new List<object>();
            var errorEntries = new List<object>();
            var successVendorCodes = new HashSet<string>(); // Use HashSet to store unique VendorCodes

            // Validate each vendor and separate valid and invalid rows
            for (int i = 0; i < vendorMasterList.Count; i++)
            {
                var vendor = vendorMasterList[i];
                var errors = ValidationHelper.ValidateModelVendors(vendor);
                if (errors.Any())
                {
                    errorEntries.Add(new
                    {
                        Status = "Failed",
                        processedItems = i + 1, // Preserve original row index (1-based index)                         
                        VendorCode = vendor.VendorCode,
                        message = "One or more validation errors have occurred. Please review the input and try again.",
                        ErrorsCode = "10000",
                        ValidationErrors = errors.Select(e => new
                        {
                            e.FieldName,
                            e.Message,
                            e.Code
                        })
                    });
                }
                else
                {
                    if (!string.IsNullOrEmpty(vendor.VendorCode))
                    {
                        //successVendorCodes.Add(vendor.VendorCode);
                        successEntries.Add(new
                        {
                            Status = "Success",
                            VendorCode = vendor.VendorCode, // Store unique VendorCodes as an array
                            Message = "Vendor records processed successfully.",
                            SuccessCode = "200",
                        }); // Store VendorCode as a string 
                    }
                }
            }

            // Add a single entry for success only if there are valid vendor codes
            //if (successVendorCodes.Count > 0)
            //{
            //    successEntries.Add(new
            //    {
            //        Status = "Success",
            //        VendorCode = successVendorCodes.ToArray(), // Convert HashSet to array for unique VendorCodes
            //        Message = "Vendor records processed successfully.",
            //        SuccessCode = "200",
            //    });
            //}

            // Insert valid vendors
            if (successEntries.Any())
            {
                var insertResponse = _vendorMasterService.UpsertVendorMaster(vendorMasterList.Where(v => !ValidationHelper.ValidateModel(v).Any()).ToList());
                if (!insertResponse.success)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Failed to insert valid vendors.",
                        data = new
                        {
                            successEntries = new List<object>(),
                            errorEntries = errorEntries
                        }
                    });
                }
            }

            // Prepare response
            return Ok(new
            {
                success = !errorEntries.Any(),
                message = errorEntries.Any()
                            ? "Some vendors processed successfully, but some failed."
                            : "All vendors processed successfully.",
                data = new
                {
                    successEntries = successEntries,
                    errorEntries = errorEntries
                }
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetVendorList")]
        public Response GetVendorList([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            Response res = new Response();
            try
            {
                res = _vendorMasterService.GetVendorList(pageNumber, pageSize);
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