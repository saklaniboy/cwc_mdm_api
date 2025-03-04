
using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Request;
using cwcerp.MDM_Models.Response;
using cwcerp.Mdm_Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace cwcerp.MDM_API.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {

        private readonly ICommonService _commonService;


        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }


        //Get all cities --Surya
        [HttpGet]
        [Route("GetCountries")]

        public Response GetCountries(int PageNumber, int PageSize)
        {
            Response res = new Response();
            try
            {
                var CountriesResponse = new CountriesResponseModel();
                CountriesResponse.Start = PageNumber;
                CountriesResponse.Length = PageSize;
                res = _commonService.GetCountries(CountriesResponse);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }

        //Get all cities --Surya
        [HttpGet]
        [Route("GetCities")]

        public Response GetCities(int PageNumber, int PageSize)
        {
            Response res = new Response();
            try
            {
                var CitiesResponse = new CitiesResponseModel();
                CitiesResponse.Start = PageNumber;
                CitiesResponse.Length = PageSize;
                res = _commonService.GetCities(CitiesResponse);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }


        /* Commented by Rakesh*/
        //Get all states --Surya
        [HttpGet]
        [Route("GetStates")]
        public Response GetStates(int PageNumber = 1, int PageSize = 100)
        {
            Response res = new Response();

            try
            {
                var StatesResponseModel = new StatesResponseModel();
                StatesResponseModel.Start = PageNumber;
                StatesResponseModel.Length = PageSize;

                res = _commonService.GetStates(StatesResponseModel);

            }
            catch (Exception ex)
            {

                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }

        //Get all Disticts --Surya
        [HttpGet]
        [Route("GetDistricts")]
        public Response GetDistricts(int PageNumber, int PageSize)
        {
            Response res = new Response();
            try
            {
                var DistrictsResponseModel = new DistrictsResponseModel();
                DistrictsResponseModel.Start = PageNumber;
                DistrictsResponseModel.Length = PageSize;
                res = _commonService.GetDistricts(DistrictsResponseModel);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }






        //Get all cities by state --Surya
        /* Parameters Modified by Rakesh*/
        [HttpGet]
        [Route("GetCitiesByState/{StateId}")]
        public Response GetAllCitiesByState(int PageNumber, int PageSize=1, int StateId=100)
        {
            Response res = new Response();
            try
            {
                var CitiesResponse = new CitiesResponseModel();
                CitiesResponse.Start = PageNumber;
                CitiesResponse.Length = PageSize;
                CitiesResponse.StateId = StateId;
                res = _commonService.GetCitiesByState(CitiesResponse);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }

        //Get all distict by state --Surya
        /* Parameters Modified by Rakesh*/
        [HttpGet]
        [Route("GetDistictsByState/{StateId}")]
        public Response GetDistictsByState(int StateId,int PageNumber=1, int PageSize=100 )
        {
            Response res = new Response();
            try {
                var DistrictsResponseModel = new DistrictsResponseModel();
                DistrictsResponseModel.Start = PageNumber;
                DistrictsResponseModel.Length = PageSize;
                DistrictsResponseModel.StateId = StateId;
                res = _commonService.GetDistictsByState(DistrictsResponseModel);
            }
            catch(Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }

        //Get all cities by distict --Surya
        /* Parameters Modified by Rakesh*/
        [HttpGet]
        [Route("GetCitiesByDistict/{DistrictId}")]
        public Response GetCitiesByDistict(int DistrictId ,int PageNumber=1, int PageSize=100 )
        {
            Response res = new Response();
            try
            {
                var CitiesResponse = new CitiesResponseModel();
                CitiesResponse.Start = PageNumber;
                CitiesResponse.Length = PageSize;
                CitiesResponse.DistrictId = DistrictId;
                res = _commonService.GetCitiesByDistict(CitiesResponse);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }

        //get city --surya
        
        [HttpGet]
        [Route("GetCityByCityId/{CityId}")]
        public Response GetCityByCityId(int CityId)
        {
            Response res = new Response();
            try
            {
                var CitiesResponse = new CitiesResponseModel();
                CitiesResponse.CityId = CityId;
                res = _commonService.GetCityByCityId(CitiesResponse);
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }

        //**Added by Rakesh **//
        //Get all states 
        //[HttpGet]
        //[Route("States")]
        //public Response GetStates()
        //{
        //    Response res = new Response();
        //    try
        //    {
        //        var StatesResponseModel = new StatesResponseModel();
        //        StatesResponseModel.Start = 1;
        //        StatesResponseModel.Length = 100;
        //        res = _commonService.GetStates(StatesResponseModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.success = false;
        //        res.message = ex.Message;
        //    }
        //    return res;
        //}
    }
}
