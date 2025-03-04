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


namespace cwcerp.MDM_Service.Service
{
    public class WarehouseMasterService : IWarehouseMasterService
    {
        Response response;
        PagingResponse pagingResponse;

        private readonly IDapperConnection _dapper;
        public WarehouseMasterService(IDapperConnection dapper)
        {
            _dapper = dapper;
        }

        //public Response AddWarehouseOffice(OfficeInfoModel warehouseOffice)
        //{
        //    using (var response = new Response())
        //    {
        //        try
        //        {
        //            var dbparams = new DynamicParameters();
        //            dbparams.Add("@LocationId", warehouseOffice.LocationId, DbType.Int32);
        //            dbparams.Add("@LocationType", warehouseOffice.LocationType, DbType.Int32);
        //            dbparams.Add("@LocationName", warehouseOffice.LocationName, DbType.String);
        //            dbparams.Add("@LocationCode", warehouseOffice.LocationCode, DbType.String);

        //            // Execute the stored procedure to add the office
        //            var rowsAffected = _dapper.Execute("USP_InsertOfficeInfo", dbparams, commandType: CommandType.StoredProcedure);

        //            if (rowsAffected > 0)
        //            {
        //                response.message = "Warehouse office added successfully.";
        //                response.success = true;
        //                response.responseData = null;
        //            }
        //            else
        //            {
        //                response.message = "Failed to add warehouse office.";
        //                response.success = false;
        //                response.responseData = null;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            response.message = "An error occurred.";
        //            response.success = false;
        //            response.responseData = ex.Message;
        //        }

        //        return response;
        //    }
        //}

        public Response AddWarehouseMaster(WarehouseMasterModel warehouseMaster)
        {
            var response = new Response();
            try
            {

                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                //string inputJson1 = "{ \"OfficeCode\": \"OFC001\", \"OfficeTypeId\": 1,\r\n\t\"OfficeName\":\"RO001\", \"AddressTypeId\": 1, \"Address\": {     \"AddressLine1\": \"123 Main St\",     \"AddressLine2\": \"Suite 456\",     \"AddressLine3\": \"\",     \"LatLong\": \"12.9716, 77.5946\",     \"CityId\": 1,     \"DistrictId\": 1,     \"StateId\": 1,     \"CountryId\": 1,     \"PinCode\": \"560001\" }, \"ContactDetails\": {     \"EmailId\": \"office@company.com\",     \"ContactNumber\": \"1234567890\",     \"FaxNumber\": \"0987654321\" }, \"BankDetails\": {     \"AccountNumber\": \"123456789\",     \"AccountHolderName\": \"Company XYZ\",     \"IFSCId\": 3 }, \"InfrastructureDetails\": {     \"TariffCategoryId\": 1,     \"BillNo\": \"BILL123\",     \"RegistrationDate\": \"2023-01-01\",     \"RegistrationNumber\": \"REG123\",     \"WdraRegistered\": true,     \"ConstructedDate\": \"2020-01-01\",     \"OpeningTime\": \"08:00\",     \"ClosingTime\": \"18:00\",     \"OpenArea\": 1000,     \"CoveredArea\": 500,     \"NumberOfGates\": 2,     \"EarthquakeZone\": 1,     \"NumberOfWeighbridge\": 1,     \"IsBuffer\": false,     \"IsLorryWeighbridge\": 1,        \"IsMoistureMeter\": 1,       \"DistanceFromRoInKm\": 50    }}";
                string inputJson1 = JsonConvert.SerializeObject(warehouseMaster);
                dbparams.Add("@input_json", inputJson1, DbType.String);

                dbparams.Add("@created_by", 123, DbType.Int32);

                dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

               // dbparams.Add("@office_id", warehouseMaster.OfficeId, DbType.Int32);

                // Execute the stored procedure
                _dapper.Insert<string>("dbo.USP_InsertWHBasicInfo", dbparams, commandType: CommandType.StoredProcedure);


                // Retrieve the output JSON
                var outputJson = dbparams.Get<string>("@output_json");

                // Parse the output JSON to determine the response
                var outputData = JsonConvert.DeserializeObject<dynamic>(outputJson);
                if (outputData.status == "success")
                {
                    response.success = true;
                    response.message = outputData.message;
                    response.responseData = outputData.responseData?.ToObject<List<Dictionary<string, object>>>();
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
                response.message = "An error occurred while adding the warehouse office.";
                response.responseData = ex.Message;
            }

            return response;
        }


        public Response AddOrUpdateWarehouseAreaDetails(WarehouseAreaDetailsModel warehouseMaster)
        {
            var response = new Response();
            try
            {
                // Convert the model to JSON input
                string jsonInput = JsonConvert.SerializeObject(warehouseMaster);

                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                dbparams.Add("@JsonInput", jsonInput, DbType.String);

                // Execute the stored procedure
                string outputJson = _dapper.Update<string>("dbo.USP_AddOrUpdateWHAreaDetails", dbparams, commandType: CommandType.StoredProcedure);

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
                response.message = "An error occurred while adding the warehouse area details.";
                response.responseData = ex.Message;
            }

            return response;
        }

        public Response AddOrUpdateWarehouseAccountDetails(WarehouseAccountDetailsModel warehouseMaster)
        {
            var response = new Response();
            try
            {
                // Convert the model to JSON input
                string jsonInput = JsonConvert.SerializeObject(warehouseMaster);

                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                dbparams.Add("@JsonInput", jsonInput, DbType.String);

                // Execute the stored procedure
                string outputJson = _dapper.Update<string>("dbo.USP_AddOrUpdateWHAccountDetails", dbparams, commandType: CommandType.StoredProcedure);

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
                response.message = "An error occurred while adding the warehouse area details.";
                response.responseData = ex.Message;
            }

            return response;
        }

        public Response AddWarehouseBankDetails(WarehouseBankDetailsModel warehouseMaster)
        {
            var response = new Response();
            try
            {
                // Convert the model to JSON input
                string jsonInput = JsonConvert.SerializeObject(warehouseMaster);

                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                dbparams.Add("@JsonInput", jsonInput, DbType.String);

                // Execute the stored procedure
                string outputJson = _dapper.Insert<string>("dbo.USP_AddWHBankDetails", dbparams, commandType: CommandType.StoredProcedure);

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
                response.message = "An error occurred while adding the warehouse area details.";
                response.responseData = ex.Message;
            }

            return response;
        }

        public Response AddWarehouseAdditionalDetails(WarehouseAdditionalDetailsModel warehouseMaster)
        {
            var response = new Response();
            try
            {

                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                //string inputJson1 = "{ \"OfficeCode\": \"OFC001\", \"OfficeTypeId\": 1,\r\n\t\"OfficeName\":\"RO001\", \"AddressTypeId\": 1, \"Address\": {     \"AddressLine1\": \"123 Main St\",     \"AddressLine2\": \"Suite 456\",     \"AddressLine3\": \"\",     \"LatLong\": \"12.9716, 77.5946\",     \"CityId\": 1,     \"DistrictId\": 1,     \"StateId\": 1,     \"CountryId\": 1,     \"PinCode\": \"560001\" }, \"ContactDetails\": {     \"EmailId\": \"office@company.com\",     \"ContactNumber\": \"1234567890\",     \"FaxNumber\": \"0987654321\" }, \"BankDetails\": {     \"AccountNumber\": \"123456789\",     \"AccountHolderName\": \"Company XYZ\",     \"IFSCId\": 3 }, \"InfrastructureDetails\": {     \"TariffCategoryId\": 1,     \"BillNo\": \"BILL123\",     \"RegistrationDate\": \"2023-01-01\",     \"RegistrationNumber\": \"REG123\",     \"WdraRegistered\": true,     \"ConstructedDate\": \"2020-01-01\",     \"OpeningTime\": \"08:00\",     \"ClosingTime\": \"18:00\",     \"OpenArea\": 1000,     \"CoveredArea\": 500,     \"NumberOfGates\": 2,     \"EarthquakeZone\": 1,     \"NumberOfWeighbridge\": 1,     \"IsBuffer\": false,     \"IsLorryWeighbridge\": 1,        \"IsMoistureMeter\": 1,       \"DistanceFromRoInKm\": 50    }}";
                string inputJson1 = JsonConvert.SerializeObject(warehouseMaster);
                dbparams.Add("@input_json", inputJson1, DbType.String);

                dbparams.Add("@created_by", 123, DbType.Int32);

                dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                // Execute the stored procedure
                _dapper.Insert<string>("dbo.USP_AddWHAdditionalDetails", dbparams, commandType: CommandType.StoredProcedure);


                // Retrieve the output JSON
                var outputJson = dbparams.Get<string>("@output_json");

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
                response.message = "An error occurred while adding the warehouse additional details.";
                response.responseData = ex.Message;
            }

            return response;
        }




        public Response GetWarehouseList(int pageNumber, int pageSize, int? locationId = null)
        {
            var response = new Response();

            try
            {
                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                dbparams.Add("@PageNumber", pageNumber, DbType.Int32);
                dbparams.Add("@PageSize", pageSize, DbType.Int32);

                // Add LocationId only if it exists
                if (locationId.HasValue)
                {
                    dbparams.Add("@LocationId", locationId, DbType.Int32);
                }

                // Execute the stored procedure and fetch results
                var warehouses = _dapper.GetAll<dynamic>(
                    "[dbo].[USP_GetWarehouseList]",
                    dbparams,
                    commandType: CommandType.StoredProcedure);

                // Set the response
                response.success = true;
                response.message = "Warehouse list retrieved successfully.";
                response.responseData = warehouses;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while retrieving the warehouse list.";
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



        public Response GetWarehouseDetailsById(int officeId)
        {
            var response = new Response();

            try
            {
                // Prepare parameters for the stored procedure
                var dbParams = new DynamicParameters();
                dbParams.Add("@OfficeId", officeId, DbType.Int32);

                // Execute the stored procedure to fetch warehouse details
                var warehouseDetails = _dapper.Get<dynamic>(
                    "dbo.USP_GetWarehouseDetailsById",
                    dbParams,
                    commandType: CommandType.StoredProcedure
                );

                // Check if data is retrieved
                if (warehouseDetails != null)
                {
                    response.success = true;
                    response.message = "Warehouse details retrieved successfully.";
                    response.responseData = warehouseDetails;
                }
                else
                {
                    response.success = false;
                    response.message = "No warehouse found for the given OfficeId.";
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while retrieving the warehouse details.";
                response.responseData = ex.Message;
            }

            return response;
        }

        public Response GetWarehouseCategories()
        {
            var response = new Response();

            try
            {
                // Prepare parameters for the stored procedure
                var dbParams = new DynamicParameters();

                // Execute the stored procedure asynchronously
                var warehouseCategories = _dapper.GetAll<dynamic>(
                     "dbo.USP_GetWarehouseCategories",
                     dbParams,
                     commandType: CommandType.StoredProcedure
                 );

                // Check if data is retrieved and contains records
                if (warehouseCategories != null )
                {
                    response.success = true;
                    response.message = "Warehouse categories retrieved successfully.";
                    response.responseData = warehouseCategories;
                }
                else
                {
                    response.success = false;
                    response.message = "No warehouse categories found.";
                    response.responseData = null;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while retrieving the warehouse categories.";
                response.responseData = ex.Message;
            }

            return response;
        }

        public Response GetWarehouseOwner()
        {
            var response = new Response();

            try
            {
                // Prepare parameters for the stored procedure
                var dbParams = new DynamicParameters();

                // Execute the stored procedure asynchronously
                var warehouseOwner = _dapper.GetAll<dynamic>(
                     "dbo.USP_GetWarehouseOwner",
                     dbParams,
                     commandType: CommandType.StoredProcedure
                 );

                // Check if data is retrieved and contains records
                if (warehouseOwner != null)
                {
                    response.success = true;
                    response.message = "Warehouse owner retrieved successfully.";
                    response.responseData = warehouseOwner;
                }
                else
                {
                    response.success = false;
                    response.message = "No warehouse owner found.";
                    response.responseData = null;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while retrieving the warehouse owner.";
                response.responseData = ex.Message;
            }

            return response;
        }

        public Response GetWarehouseTypes()
        {
            var response = new Response();

            try
            {
                // Prepare parameters for the stored procedure
                var dbParams = new DynamicParameters();

                // Execute the stored procedure asynchronously
                var warehouseTypes = _dapper.GetAll<dynamic>(
                     "dbo.USP_GetWarehouseTypes",
                     dbParams,
                     commandType: CommandType.StoredProcedure
                 );

                // Check if data is retrieved and contains records
                if (warehouseTypes != null)
                {
                    response.success = true;
                    response.message = "Warehouse types retrieved successfully.";
                    response.responseData = warehouseTypes;
                }
                else
                {
                    response.success = false;
                    response.message = "No warehouse types found.";
                    response.responseData = null;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while retrieving the warehouse types.";
                response.responseData = ex.Message;
            }

            return response;
        }



        public Response AddWarehouse(WarehouseMasterModel warehouseMaster)
        {
            throw new NotImplementedException();
        }
       
    }
}
