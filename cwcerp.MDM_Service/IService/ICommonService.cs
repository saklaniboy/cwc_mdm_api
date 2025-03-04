

using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Response;

namespace cwcerp.Mdm_Service.IService
{
    public interface ICommonService
    {
        Response GetCountries(CountriesResponseModel model);
        Response GetCities(CitiesResponseModel model);
        Response GetStates(StatesResponseModel model);
        Response GetDistricts(DistrictsResponseModel model);
        Response GetCitiesByDistict(CitiesResponseModel model);
        Response GetCitiesByState(CitiesResponseModel model);
        Response GetDistictsByState(DistrictsResponseModel model);
        Response GetCityByCityId(CitiesResponseModel model);
    }




}
