using cwcerp.MDM_Service.IService;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Response;
using cwcerp.Mdm_Repository;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using cwcerp.Mdm_Models.Request;


namespace cwcerp.MDM_Service.Service
{
    public class PcsOfficeService : IPcsOfficeService
    {
        Response response;
        PagingResponse pagingResponse;

        private readonly IDapperConnection _dapper;
        public PcsOfficeService(IDapperConnection dapper)
        {
            _dapper = dapper;
        }

        public Response AddPcsBasicInfo(PcsBasicInfoModel pcsOffice)
        {
            var response = new Response();
            try
            {

                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                //string inputJson1 = "{ \"OfficeCode\": \"OFC001\", \"OfficeTypeId\": 1,\r\n\t\"OfficeName\":\"RO001\", \"AddressTypeId\": 1, \"Address\": {     \"AddressLine1\": \"123 Main St\",     \"AddressLine2\": \"Suite 456\",     \"AddressLine3\": \"\",     \"LatLong\": \"12.9716, 77.5946\",     \"CityId\": 1,     \"DistrictId\": 1,     \"StateId\": 1,     \"CountryId\": 1,     \"PinCode\": \"560001\" }, \"ContactDetails\": {     \"EmailId\": \"office@company.com\",     \"ContactNumber\": \"1234567890\",     \"FaxNumber\": \"0987654321\" }, \"BankDetails\": {     \"AccountNumber\": \"123456789\",     \"AccountHolderName\": \"Company XYZ\",     \"IFSCId\": 3 }, \"InfrastructureDetails\": {     \"TariffCategoryId\": 1,     \"BillNo\": \"BILL123\",     \"RegistrationDate\": \"2023-01-01\",     \"RegistrationNumber\": \"REG123\",     \"WdraRegistered\": true,     \"ConstructedDate\": \"2020-01-01\",     \"OpeningTime\": \"08:00\",     \"ClosingTime\": \"18:00\",     \"OpenArea\": 1000,     \"CoveredArea\": 500,     \"NumberOfGates\": 2,     \"EarthquakeZone\": 1,     \"NumberOfWeighbridge\": 1,     \"IsBuffer\": false,     \"IsLorryWeighbridge\": 1,        \"IsMoistureMeter\": 1,       \"DistanceFromRoInKm\": 50    }}";
                string inputJson1 = JsonConvert.SerializeObject(pcsOffice);
                dbparams.Add("@input_json", inputJson1, DbType.String);

                dbparams.Add("@created_by", 1, DbType.Int32);

                dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                // Execute the stored procedure
                _dapper.Insert<string>("dbo.USP_InsertPCSBasicInfo", dbparams, commandType: CommandType.StoredProcedure);


                // Retrieve the output JSON
                var outputJson = dbparams.Get<string>("@output_json");

                // Parse the output JSON to determine the response
                var outputData = JsonConvert.DeserializeObject<dynamic>(outputJson);
                if (outputData.status == "success")
                {
                    response.success = true;
                    response.message = outputData.message;
                    response.responseData = (int)outputData.OfficeId;
                }
                else
                {
                    response.success = false;
                    response.message = outputData.message;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while adding the pcs office.";
                response.responseData = ex.Message;
            }

            return response;
        }


        public Response AddOrUpdatePcsAccountDetails(PcsAccountDetails pcsOffice)
        {
            var response = new Response();
            try
            {

                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                //string inputJson1 = "{ \"OfficeCode\": \"OFC001\", \"OfficeTypeId\": 1,\r\n\t\"OfficeName\":\"RO001\", \"AddressTypeId\": 1, \"Address\": {     \"AddressLine1\": \"123 Main St\",     \"AddressLine2\": \"Suite 456\",     \"AddressLine3\": \"\",     \"LatLong\": \"12.9716, 77.5946\",     \"CityId\": 1,     \"DistrictId\": 1,     \"StateId\": 1,     \"CountryId\": 1,     \"PinCode\": \"560001\" }, \"ContactDetails\": {     \"EmailId\": \"office@company.com\",     \"ContactNumber\": \"1234567890\",     \"FaxNumber\": \"0987654321\" }, \"BankDetails\": {     \"AccountNumber\": \"123456789\",     \"AccountHolderName\": \"Company XYZ\",     \"IFSCId\": 3 }, \"InfrastructureDetails\": {     \"TariffCategoryId\": 1,     \"BillNo\": \"BILL123\",     \"RegistrationDate\": \"2023-01-01\",     \"RegistrationNumber\": \"REG123\",     \"WdraRegistered\": true,     \"ConstructedDate\": \"2020-01-01\",     \"OpeningTime\": \"08:00\",     \"ClosingTime\": \"18:00\",     \"OpenArea\": 1000,     \"CoveredArea\": 500,     \"NumberOfGates\": 2,     \"EarthquakeZone\": 1,     \"NumberOfWeighbridge\": 1,     \"IsBuffer\": false,     \"IsLorryWeighbridge\": 1,        \"IsMoistureMeter\": 1,       \"DistanceFromRoInKm\": 50    }}";
                string inputJson1 = JsonConvert.SerializeObject(pcsOffice);

                dbparams.Add("@input_json", inputJson1, DbType.String);

                dbparams.Add("@created_by", 1, DbType.Int32);

                dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                _dapper.Insert<string>("dbo.USP_UpsertPCSAccountInfo", dbparams, commandType: CommandType.StoredProcedure);

                // Retrieve the output JSON
                var outputJson = dbparams.Get<string>("@output_json");

                Console.WriteLine("Output JSON: " + outputJson);

                // Parse the output JSON to determine the response
                var outputData = JsonConvert.DeserializeObject<dynamic>(outputJson);

                if (outputData.status == "success")
                {
                    response.success = true;
                    response.message = outputData.message;
                }
                else
                {
                    response.success = false;
                    response.message = outputData.message;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while adding the pcs acount details.";
                response.responseData = ex.Message;
            }

            return response;
        }

        public Response AddPcsBankDetails(PcsBankDetailsModel pcsOffice)
        {
            var response = new Response();
            try
            {
                // Convert the model to JSON input
                string jsonInput = JsonConvert.SerializeObject(pcsOffice);

                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();

                dbparams.Add("@input_json", jsonInput, DbType.String);

                dbparams.Add("@created_by", 1, DbType.Int32);

                dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                _dapper.Insert<string>("dbo.USP_UpsertPCSBankInfo", dbparams, commandType: CommandType.StoredProcedure);

                // Retrieve the output JSON
                var outputJson = dbparams.Get<string>("@output_json");

                Console.WriteLine("Output JSON: " + outputJson);

                // Parse the output JSON to determine the response
                var outputData = JsonConvert.DeserializeObject<dynamic>(outputJson);

                if (outputData.Success == 1)
                {
                    response.success = true;
                    response.message = outputData.Message;
                }
                else
                {
                    response.success = false;
                    response.message = outputData.Message ?? "Unknown error occurred.";
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while adding the pcs bank details.";
                response.responseData = ex.Message;
            }

            return response;
        }

        //public Response AddPcsBankDetails(PcsBankDetailsModel pcsOffice)
        //{
        //    var response = new Response();
        //    try
        //    {
        //        // Prepare parameters for the stored procedure
        //        var dbparams = new DynamicParameters();

        //        string inputJson1 = JsonConvert.SerializeObject(pcsOffice);

        //        dbparams.Add("@input_json", inputJson1, DbType.String);

        //        //dbparams.Add("@created_by", 1, DbType.Int32);

        //        dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

        //        // Execute the stored procedure
        //        _dapper.Insert<string>("dbo.USP_UpsertPCSBankInfo", dbparams, commandType: CommandType.StoredProcedure);

        //        // Retrieve the output JSON
        //        var outputJson = dbparams.Get<string>("@output_json");

        //        Console.WriteLine("Output JSON: " + outputJson);

        //        // Parse the output JSON to determine the response
        //        var outputData = JsonConvert.DeserializeObject<dynamic>(outputJson);
        //        if (outputData.status == "success")
        //        {
        //            response.success = true;
        //            response.message = outputData.message;
        //        }
        //        else
        //        {
        //            response.success = false;
        //            response.message = outputData.message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.message = "An error occurred while adding the pcs area details.";
        //        response.responseData = ex.Message;
        //    }

        //    return response;
        //}

        public Response AddPcsAdditionalDetails(PcsAdditionalDetailsModel pcsOffice)
        {
            var response = new Response();
            try
            {   
                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();

                string inputJson1 = JsonConvert.SerializeObject(pcsOffice);

                dbparams.Add("@input_json", inputJson1, DbType.String);

                dbparams.Add("@created_by", 1, DbType.Int32);

                dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                // Execute the stored procedure
                _dapper.Insert<string>("dbo.USP_UpsertPCSAdditionalInfo", dbparams, commandType: CommandType.StoredProcedure);

                // Retrieve the output JSON
                var outputJson = dbparams.Get<string>("@output_json");

                Console.WriteLine("Output JSON: " + outputJson);

                // Parse the output JSON to determine the response
                var outputData = JsonConvert.DeserializeObject<dynamic>(outputJson);
                if (outputData.status == "success")
                {
                    response.success = true;
                    response.message = outputData.message;
                }
                else
                {
                    response.success = false;
                    response.message = outputData.message;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while adding the pcs additional details.";
                response.responseData = ex.Message;
            }

            return response;
        }




        public Response GetPcsList(int? pageNumber, int? pageSize)
        {
            var response = new Response();

            try
            {
                int totalRecord = 0;
                int totalPages = 0;
                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();

                dbparams.Add("@PageNumber", pageNumber ?? (object)DBNull.Value, DbType.Int32);
                dbparams.Add("@PageSize", pageSize ?? (object)DBNull.Value, DbType.Int32);

                dbparams.Add("@totalrows", dbType: DbType.Int32, direction: ParameterDirection.Output);
                dbparams.Add("@TotalPages", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Execute the stored procedure and fetch results
                var pcs = _dapper.GetAll<dynamic>(
                    "[dbo].[USP_GetPcsList]",
                    dbparams,
                    commandType: CommandType.StoredProcedure);

                totalRecord = dbparams.Get<int>("@totalrows");
                totalPages = dbparams.Get<int>("@TotalPages");

                if (pcs != null)
                {
                    response.message = "PCS list retrieved successfully.";
                    response.success = true;
                    response.responseData = pcs;
                    response.RecordsTotal = totalPages;
                    response.RecordsFiltered = totalRecord;
                }
                else
                {
                    response.message = "fail";
                    response.success = false;
                    response.responseData = null;
                    response.RecordsTotal = totalPages;
                    response.RecordsFiltered = totalRecord;
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while retrieving the pcs list.";
                response.responseData = ex.Message;
            }

            return response;
        }

        public Response UpdateOfficeStatus(WOfficeStatus request)
        {
            var response = new Response();

            try
            {
                var dbparams = new DynamicParameters();

                // Add all parameters from the OfficeStatus model
                dbparams.Add("@OfficeId", request.OfficeId, DbType.Int32);
                dbparams.Add("@OfficeTypeId", request.OfficeTypeId, DbType.Int32);
                dbparams.Add("@ProcessId", request.ProcessId, DbType.Int32);
                dbparams.Add("@StatusId", request.StatusId, DbType.Int32);
                dbparams.Add("@Reason", request.Reason, DbType.String);
                dbparams.Add("@StepId", request.StepId, DbType.Int32);
                dbparams.Add("@OutputJson", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                _dapper.Insert<string>("dbo.USP_UpsertOfficeStatus", dbparams, commandType: CommandType.StoredProcedure);

                // Retrieve the output JSON
                var outputJson = dbparams.Get<string>("@OutputJson");

                var outputData = JsonConvert.DeserializeObject<dynamic>(outputJson);
                if (outputData.status == "success")
                {
                    response.success = true;
                    response.message = outputData.message;
                    response.responseData = outputData.responseData;
                }
                else
                {
                    response.success = false;
                    response.message = outputData.message;
                }
            }
            catch (Exception ex)
            {
                // Exception handling
                response.success = false;
                response.message = "fail";
                response.responseData = ex.Message;
            }

            return response;
        }

        public Response GetPcsDetailsById(int officeId)
        {
            var response = new Response();

            try
            {
                // Prepare parameters for the stored procedure
                var dbParams = new DynamicParameters();
                dbParams.Add("@OfficeId", officeId, DbType.Int32);

                // Execute the stored procedure to fetch pcs details
                var pcsDetails = _dapper.Get<dynamic>(
                    "dbo.USP_GetPcsDetailsById",
                    dbParams,
                    commandType: CommandType.StoredProcedure
                );

                // Check if data is retrieved
                if (pcsDetails != null)
                {
                    response.success = true;
                    response.message = "Pcs details retrieved successfully.";
                    response.responseData = pcsDetails;
                }
                else
                {
                    response.success = false;
                    response.message = "No Pcs found for the given OfficeId.";
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while retrieving the pcs details.";
                response.responseData = ex.Message;
            }

            return response;
        }
    }
}
