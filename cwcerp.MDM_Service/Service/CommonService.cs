
using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Response;
using cwcerp.Mdm_Repository;
using cwcerp.Mdm_Service.IService;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;

namespace cwcerp.Mdm_Service.Service
{
    public class CommonService : ICommonService
    {
        Response response;
        PagingResponse pagingResponse;

        private readonly IDapperConnection _dapper;
        public CommonService(IDapperConnection dapper)
        {
            _dapper = dapper;
        }


        public Response GetCountries(CountriesResponseModel model)
        {
            using (response = new Response())
            {

                try
                {
                    var dbparams = new DynamicParameters();
                    //dbparams.Add("@search", model.Search, DbType.String);
                    dbparams.Add("@PageNumber", model.Start, DbType.Int32);
                    dbparams.Add("@PageSize", model.Length, DbType.Int32);
                    dbparams.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    List<CountriesResponseModel> countyRes = new List<CountriesResponseModel>();

                    countyRes = _dapper.GetAll<CountriesResponseModel>("dbo.USP_GetCountries", dbparams, commandType: CommandType.StoredProcedure);
                    int totalRecords = dbparams.Get<int>("@TotalRecords");
                    if (countyRes.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = countyRes;
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
        }


        public Response GetCities(CitiesResponseModel model)
        {
            using (response = new Response())
            {

                try
                {
                    var dbparams = new DynamicParameters();
                    //dbparams.Add("@search", model.Search, DbType.String);
                    dbparams.Add("@PageNumber", model.Start, DbType.Int32);
                    dbparams.Add("@PageSize", model.Length, DbType.Int32);
                    dbparams.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    List<CitiesResponseModel> citiesRes = new List<CitiesResponseModel>();

                    citiesRes = _dapper.GetAll<CitiesResponseModel>("dbo.USP_GetCities", dbparams, commandType: CommandType.StoredProcedure);
                    int totalRecords = dbparams.Get<int>("@TotalRecords");
                    if (citiesRes.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = citiesRes;
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
                    response.responseData =  ex.Message;
                }
                
                return response;
            }
        }



        public Response GetStates(StatesResponseModel model)
        {
            using (response = new Response())
            {
                try
                {

                    var dbparams = new DynamicParameters();
                    dbparams.Add("@PageNumber", model.Start, DbType.Int32);
                    dbparams.Add("@PageSize", model.Length, DbType.Int32);
                    dbparams.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    List<StatesResponseModel> stateRes = new List<StatesResponseModel>();

                    stateRes = _dapper.GetAll<StatesResponseModel>("dbo.USP_GetStates", dbparams, commandType: CommandType.StoredProcedure);
                    if (stateRes.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = stateRes;

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
        }



        public Response GetDistricts(DistrictsResponseModel model)
        {
            using (response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@PageNumber", model.Start, DbType.Int32);
                    dbparams.Add("@PageSize", model.Length, DbType.Int32);
                    dbparams.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    List<DistrictsResponseModel> distictRes = new List<DistrictsResponseModel>();
                    distictRes = _dapper.GetAll<DistrictsResponseModel>("dbo.USP_GetDistricts", dbparams, commandType: CommandType.StoredProcedure);
                    if (distictRes.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = distictRes;
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
        }


        public Response GetCitiesByDistict(CitiesResponseModel model)
        {
            using (response = new Response())
            {
                try
                {

                    var dbparams = new DynamicParameters();
                    dbparams.Add("@PageNumber", model.Start, DbType.Int32);
                    dbparams.Add("@PageSize", model.Length, DbType.Int32);
                    dbparams.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    dbparams.Add("@DistrictId", model.DistrictId, DbType.Int32, ParameterDirection.Input);
                    List<CitiesResponseModel> cityRes = new List<CitiesResponseModel>();
                    cityRes = _dapper.GetAll<CitiesResponseModel>("dbo.USP_GetCitiesByDistict", dbparams, commandType: CommandType.StoredProcedure);
                    if (cityRes.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = cityRes;
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
        }

        public Response GetCitiesByState(CitiesResponseModel model)
        {
            using (response = new Response())
            {
                try
                {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@PageNumber", model.Start, DbType.Int32);
                    dbparams.Add("@PageSize", model.Length, DbType.Int32);
                    dbparams.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    dbparams.Add("@StateId", model.StateId, DbType.Int32, ParameterDirection.Input);
                    List<CitiesResponseModel> cityRes = new List<CitiesResponseModel>();
                    cityRes = _dapper.GetAll<CitiesResponseModel>("dbo.USP_GetCitiesByState", dbparams, commandType: CommandType.StoredProcedure);
                    if (cityRes.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = cityRes;
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
        }


        public Response GetDistictsByState(DistrictsResponseModel model)
        {
            using (response = new Response())
            {

               try
               {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@PageNumber", model.Start, DbType.Int32);
                    dbparams.Add("@PageSize", model.Length, DbType.Int32);
                    dbparams.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    dbparams.Add("@StateId", model.StateId, DbType.Int32, ParameterDirection.Input);
                    List<DistrictsResponseModel> distictRes = new List<DistrictsResponseModel>();
                    distictRes = _dapper.GetAll<DistrictsResponseModel>("dbo.USP_GetDistrictsByState", dbparams, commandType: CommandType.StoredProcedure);
                    if(distictRes.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = distictRes;

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
        }

        public Response GetCityByCityId(CitiesResponseModel model)
        {
            using (response = new Response())
            {
                try 
                {
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@CityID", model.CityId, DbType.Int32, ParameterDirection.Input);
                    List<CitiesResponseModel> cityRes = new List<CitiesResponseModel>();
                    cityRes = _dapper.GetAll<CitiesResponseModel>("dbo.USP_GetCityByCityId", dbparams, commandType: CommandType.StoredProcedure);
                    if (cityRes.Count != 0)
                    {
                        response.message = "Success";
                        response.success = true;
                        response.responseData = cityRes;
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
        }

       
    }
}
