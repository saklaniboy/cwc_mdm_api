using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;
using cwcerp.Mdm_Repository;
using cwcerp.Mdm_Service.IService;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace cwcerp.Mdm_Service.Service
{
    public class RegionOfficeService : IRegionOfficeService
    {
        Response response;
        PagingResponse pagingResponse;

        private readonly IDapperConnection _dapper;
        public RegionOfficeService(IDapperConnection dapper)
        {
            _dapper = dapper;
        }

        public Response GetRoList(int? pageNumber, int? pageSize, int? statusId)
        {
            using (response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();                    
                    if (pageNumber != null){
                        dbparams.Add("@PageNumber", pageNumber, DbType.Int32);                        
                    }else{
                        dbparams.Add("@PageNumber", DBNull.Value, DbType.Int32);
                    }

                    if (pageSize != null){
                        dbparams.Add("@PageSize", pageSize, DbType.Int32);
                    }else{
                        dbparams.Add("@PageSize", DBNull.Value, DbType.Int32);
                    }

                    if (statusId != null){
                        dbparams.Add("@StatusId", statusId, DbType.Int32);
                    }else{
                        dbparams.Add("@StatusId", DBNull.Value, DbType.Int32);
                    }
                    dbparams.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    List<RegionOfficeList> officeList = new List<RegionOfficeList>();

                    officeList = _dapper.GetAll<RegionOfficeList>("dbo.USP_GetROList", dbparams, commandType: CommandType.StoredProcedure);
                    if (officeList.Count != 0)
                    {
                        var totalRecords = dbparams.Get<int>("@TotalRecords");

                        response.message = "Success";
                        response.success = true;
                        response.responseData = new { OfficeList = officeList, TotalRecords = totalRecords };
                    }
                    else
                    {
                        response.message = "fail";
                        response.success = false;
                        response.responseData = null;
                    }
                }
                catch (Exception ex)
                {
                    response.message = "fail";
                    response.success = false;
                    response.responseData = ex.Message;
                }
                return response;
            }
        }

        public Response GetRoInfo(int officeId)
        {
            using (var response = new Response())
            {
                try
                {
                    // Create dynamic parameters for the stored procedure call
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@OfficeId", officeId, DbType.Int32); // Input parameter
                    dbparams.Add("@OutputJson", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000); // Output parameter
                    
                    _dapper.GetAll<RegionOfficeResponse>("dbo.USP_GetROInfo", dbparams, commandType: CommandType.StoredProcedure);

                    // Retrieve the output JSON
                    var outputJson = dbparams.Get<string>("@OutputJson");
                    if (string.IsNullOrEmpty(outputJson))
                    {
                        response.success = false;
                        response.message = "No data received.";
                        return response;
                    }
                    else
                    {
                        response.success = true;
                        response.message = "Data retrieved successfully";
                        response.responseData = outputJson;
                    }
                }
                catch (Exception ex)
                {
                    response.message = "fail";
                    response.success = false;
                    response.responseData = ex.Message;
                }

                return response;
            }
        }


        public Response BasicInfo(RoBasicInfo request)
        {
            using (var response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@input_json", JsonConvert.SerializeObject(request), DbType.String);
                    dbparams.Add("@created_by", 1, DbType.Int32);
                    dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    _dapper.Insert<string>("dbo.USP_UpsertROBasicInfo", dbparams, commandType: CommandType.StoredProcedure);

                    // Retrieve the output JSON
                    var outputJson = dbparams.Get<string>("@output_json");

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
                    response.message = "fail";
                    response.success = false;
                    response.responseData = ex.Message;
                }

                return response;
            }
        }


        public Response BillingAddressInfo(RoBillingInfo request)
        {
            using (var response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@input_json", JsonConvert.SerializeObject(request), DbType.String);
                    dbparams.Add("@created_by", 1, DbType.Int32);
                    dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    _dapper.Insert<string>("dbo.USP_UpsertROBillingInfo", dbparams, commandType: CommandType.StoredProcedure);

                    // Retrieve the output JSON
                    var outputJson = dbparams.Get<string>("@output_json");

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
                    response.message = "fail";
                    response.success = false;
                    response.responseData = ex.Message;
                }

                return response;
            }
        }

        public Response BankInfo(RoBankInfo request)
        {
            using (var response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@input_json", JsonConvert.SerializeObject(request), DbType.String);
                    dbparams.Add("@created_by", 1, DbType.Int32);
                    dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    _dapper.Insert<string>("dbo.USP_UpsertROBankInfo", dbparams, commandType: CommandType.StoredProcedure);

                    // Retrieve the output JSON
                    var outputJson = dbparams.Get<string>("@output_json");

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
                    response.message = "fail";
                    response.success = false;
                    response.responseData = ex.Message;
                }

                return response;
            }
        }

        public Response ContactInfo(RoContactInfo request)
        {
            using (var response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();                    
                    dbparams.Add("@input_json", JsonConvert.SerializeObject(request), DbType.String);
                    dbparams.Add("@created_by", 1, DbType.Int32);
                    dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    _dapper.Insert<string>("dbo.USP_UpsertROContactInfo", dbparams, commandType: CommandType.StoredProcedure);

                    // Retrieve the output JSON
                    var outputJson = dbparams.Get<string>("@output_json");

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
                    response.message = "fail";
                    response.success = false;
                    response.responseData = ex.Message;
                }

                return response;
            }
        }

        public Response OfficeStatus(OfficeStatus request)
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
                dbparams.Add("@ActionBy", request.ActionBy, DbType.Int32);
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


    }
}
