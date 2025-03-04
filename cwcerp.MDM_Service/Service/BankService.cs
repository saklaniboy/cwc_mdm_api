using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;
using cwcerp.Mdm_Repository;
using cwcerp.Mdm_Service.IService;
using Dapper;
using Newtonsoft.Json;
using System.Data;

namespace cwcerp.Mdm_Service.Service
{
    public class BankService : IBankService
    {
        Response response;
        PagingResponse pagingResponse;

        private readonly IDapperConnection _dapper;
        public BankService(IDapperConnection dapper)
        {
            _dapper = dapper;
        }

        public Response GetBankList()
        {
            using (response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    List<BankResponse> bankList = new List<BankResponse>();

                    bankList = _dapper.GetAll<BankResponse>("dbo.USP_GetBankList", dbparams, commandType: CommandType.StoredProcedure);
                    if (bankList.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = bankList;
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

        public Response GetBankIFSC(int bankId)
        {
            using (response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@bankId", bankId, DbType.Int32);
                    List<BankIFSC> bankIfsc = new List<BankIFSC>();

                    bankIfsc = _dapper.GetAll<BankIFSC>("dbo.USP_GetBankIFSC", dbparams, commandType: CommandType.StoredProcedure);
                    if (bankIfsc.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = bankIfsc;
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

    }
}
