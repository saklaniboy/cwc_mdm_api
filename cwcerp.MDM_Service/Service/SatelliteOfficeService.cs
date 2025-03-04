﻿using cwcerp.Mdm_Models.Request;
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
    public class SatelliteOfficeService : ISatelliteOfficeService
    {
        Response response;
        PagingResponse pagingResponse;

        private readonly IDapperConnection _dapper;
        public SatelliteOfficeService(IDapperConnection dapper)
        {
            _dapper = dapper;
        }

        public Response GetSoList(int pageNumber, int pageSize)
        {
            using (response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@PageNumber", pageNumber, DbType.Int32);
                    dbparams.Add("@PageSize", pageSize, DbType.Int32);
                    dbparams.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    List<SatelliteOfficeList> officeList = new List<SatelliteOfficeList>();

                    officeList = _dapper.GetAll<SatelliteOfficeList>("dbo.USP_GetSOList", dbparams, commandType: CommandType.StoredProcedure);
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

        public Response GetSoInfo(int officeId)
        {
            using (var response = new Response())
            {
                try
                {
                    // Create dynamic parameters for the stored procedure call
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@OfficeId", officeId, DbType.Int32); // Input parameter
                    dbparams.Add("@OutputJson", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000); // Output parameter

                    _dapper.GetAll<SatelliteOfficeResponse>("dbo.USP_GetSOInfo", dbparams, commandType: CommandType.StoredProcedure);
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


        public Response GetSoDetails(int officeId)
        {
            using (var response = new Response())
            {
                try
                {
                    // Create dynamic parameters for the stored procedure call
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@OfficeId", officeId, DbType.Int32); // Input parameter
                    dbparams.Add("@OutputJson", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000); // Output parameter

                    _dapper.GetAll<SatelliteOfficeResponse>("master.USP_GetOfficeDetails", dbparams, commandType: CommandType.StoredProcedure);

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
                    response.message = "fail";
                    response.success = false;
                    response.responseData = ex.Message;
                }

                return response;
            }
        }


        public Response BasicInfo(SoBasicInfo request)
        {
            using (var response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@input_json", JsonConvert.SerializeObject(request), DbType.String);
                    dbparams.Add("@created_by", 1, DbType.Int32);
                    dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    _dapper.Insert<string>("dbo.USP_UpsertSOBasicInfo", dbparams, commandType: CommandType.StoredProcedure);

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


        public Response BillingAddressInfo(SoBillingInfo request)
        {
            using (var response = new Response())
            {
                try
                {

                    var dbparams = new DynamicParameters();
                    var jsonRequest = JsonConvert.SerializeObject(request);
                    dbparams.Add("@input_json", jsonRequest, DbType.String);
                    dbparams.Add("@created_by", 1, DbType.Int32);
                    dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    _dapper.Insert<string>("dbo.USP_UpsertSOBillingInfo", dbparams, commandType: CommandType.StoredProcedure);

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

        public Response BankInfo(SoBankInfo request)
        {
            using (var response = new Response())
            {
                try
                {
                    var json = JsonConvert.SerializeObject(request);
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@input_json", JsonConvert.SerializeObject(request), DbType.String);
                    dbparams.Add("@created_by", 1, DbType.Int32);
                    dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    

                    _dapper.Insert<string>("dbo.USP_UpsertSOBankInfo", dbparams, commandType: CommandType.StoredProcedure);

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

        public Response ContactInfo(SoContactInfo request)
        {
            using (var response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@input_json", JsonConvert.SerializeObject(request), DbType.String);
                    dbparams.Add("@created_by", 1, DbType.Int32);
                    dbparams.Add("@output_json", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    _dapper.Insert<string>("dbo.USP_UpsertSOContactInfo", dbparams, commandType: CommandType.StoredProcedure);

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
                dbparams.Add("@StepId", request.StepId, DbType.Int32);
                dbparams.Add("@Reason", request.Reason, DbType.String);
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
