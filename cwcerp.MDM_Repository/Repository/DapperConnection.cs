using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using static Dapper.SqlMapper;


namespace cwcerp.Mdm_Repository
{
    public class DapperConnection : IDapperConnection
    {
        private IConfiguration _config;

        public DapperConnection(IConfiguration config)
        {
            _config = config;
        }
        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString"));

        }


        #region NON ASYNC methods
        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString")))
            {
                return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
            }
        }
       

        public T GetQuery<T>(string query, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString")))
            {
                return db.Query<T>(query).FirstOrDefault();
            }
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString")))
            {
                return db.Query<T>(sp, parms, commandType: commandType).ToList();
            }
        }



        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString"));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var trans = db.BeginTransaction())
                {
                    try
                    {
                        result = db.Query<T>(sp, parms, commandType: commandType, transaction: trans).FirstOrDefault();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;

        }


        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString"));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var trans = db.BeginTransaction())
                {
                    try
                    {
                        result = db.Query<T>(sp, parms, commandType: commandType, transaction: trans).FirstOrDefault();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public List<T> GetAllForQuery<T>(string query, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString")))
            {
                return db.Query<T>(query, commandType: commandType).ToList();
            }
        }
        public GridReader QueryMultiple(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString"));
            return db.QueryMultiple(sp, parms, commandType: commandType);
        }
        #endregion


        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            //throw new NotImplementedException();
            IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString"));
            // Execute stored procedure using Dapper
            var result =  db.QueryAsync(sp, parms, commandType: commandType);

            // Load the result into a DataTable
            //    var dataTable = new DataTable();
            //dataTable.Load(result.ToDataReader());

            return 0;
        }

        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString")))
            {
                var data = db.Query<T>(sp, parms, commandType: commandType);
                return data.FirstOrDefault();
            }
        }
        public async Task<T> GetQueryAsync<T>(string query, object parms, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString")))
            {
                var data = db.Query<T>(query, parms, commandType: commandType);
                return data.FirstOrDefault();
            }
        }

        public async Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString")))
            {
                var data = db.Query<T>(sp, parms, commandType: commandType);
                return data.ToList();
            }
        }



        public async Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString"));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var trans = db.BeginTransaction())
                {
                    try
                    {
                        var data = await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: trans);
                        result = data.FirstOrDefault();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;

        }


        public async Task<T> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString"));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var trans = db.BeginTransaction())
                {
                    try
                    {
                        var data = await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: trans);
                        result = data.FirstOrDefault();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public async Task<GridReader> QueryMultipleAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString"));
            return await db.QueryMultipleAsync(sp, parms, commandType: commandType);
        }

        public async Task<dynamic> ExecuteStoredProcedureAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(_config.GetValue<string>("ConnectionStrings:DBConnectionString"));
            // Execute stored procedure using Dapper
            var result = await db.QueryAsync(sp, parms, commandType: commandType);

                // Load the result into a DataTable
            //    var dataTable = new DataTable();
            //dataTable.Load(result.ToDataReader());

                return result;
            
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<T> GetAll<T>(int pageNumber, string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new NotImplementedException();
        }
    }
}
