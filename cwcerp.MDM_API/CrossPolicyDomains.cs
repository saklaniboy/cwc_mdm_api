using Dapper;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace cwcerp.MDM_API
{
    public  class CrossPolicyDomains
    {
        public CrossPolicyDomains(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public  string[] GetCrossPolicyDomains()
        {
            string[] valuesArray = new string[] { };
            try
            {
                IDbConnection dbConnection = new SqlConnection(Configuration.GetSection("ConnectionStrings:DBConnectionString").Get<string>());
                string storedProcedureName = "sp_Get_ClientMasterAll"; // Replace with your actual stored procedure name
                var parameters = new DynamicParameters();
                object results = dbConnection.Query<object>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
                var resultsed = results.GetType();
                string json = JsonConvert.SerializeObject(results, Formatting.Indented);
                JArray jsonArray = JArray.Parse(json);
                valuesArray = jsonArray.Select(j => j["ReferenceId"].ToString()).ToArray();

                return valuesArray;
            }
            catch
            {
                return valuesArray;
            }
        }
    }
}
