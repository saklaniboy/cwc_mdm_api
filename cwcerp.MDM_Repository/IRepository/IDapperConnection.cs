using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace cwcerp.Mdm_Repository
{
    public interface IDapperConnection : IDisposable
    {
        DbConnection GetDbconnection();

        #region Non Async Methods
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(int pageNumber, string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAllForQuery<T>(string sp, CommandType commandType = CommandType.Text);
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T GetQuery<T>(string query, CommandType commandType = CommandType.Text);
        GridReader QueryMultiple(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        #endregion

        Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> GetQueryAsync<T>(string query, object parms, CommandType commandType = CommandType.Text);
        Task<GridReader> QueryMultipleAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<dynamic> ExecuteStoredProcedureAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string v, DynamicParameters dbparams, CommandType commandType);
    }
}
