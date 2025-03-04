using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Response;
using cwcerp.Mdm_Repository;
using cwcerp.Mdm_Service.IService;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Reflection;

namespace cwcerp.Mdm_Service.Service
{
    public class AuditLogService : IAuditLogService
    {
        Response response;
        PagingResponse pagingResponse;
        private readonly IDapperConnection _dapper;

        public AuditLogService(IDapperConnection dapper)
        {
            _dapper = dapper;
        }

        /*public Response GetAuditLogs()
        {
            using (response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    List<AuditLogModel> logRes = new List<AuditLogModel>();
                    logRes = _dapper.GetAll<AuditLogModel>("master.USP_GetAuditLogs", dbparams, commandType: CommandType.StoredProcedure);
                    if (logRes.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = logRes;
                    }
                    else
                    {
                        response.message = "fail";
                        response.success = false;
                        response.responseData = "";
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
        }*/


        // Helper method to create a response
        private Response CreateResponse(bool success, string message, object data)
        {
            return new Response
            {
                success = success,
                message = message,
                responseData = data
            };
        }

        public Response GetAuditLogs()
        {
            try
            {
                var dbparams = new DynamicParameters();
                List<AuditLogModel> logRes = _dapper.GetAll<AuditLogModel>("master.USP_GetAuditLogs", dbparams, commandType: CommandType.StoredProcedure);

                if (logRes.Any())
                {
                    return CreateResponse(true, "Success", logRes);
                }
                else
                {
                    return CreateResponse(false, "No data found", null);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(false, "Error", ex.Message);
            }
        }

    }
}
