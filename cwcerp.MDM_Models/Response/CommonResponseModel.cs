using Newtonsoft.Json;

namespace cwcerp.MDM_Models.Response
{
    public class CountriesResponseModel
    {

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryShortName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }

        [JsonProperty(PropertyName = "start")]
        public int? Start { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int? Length { get; set; }

        public string? Search { get; set; }

    }
    public class CitiesResponseModel
    {

        public int CityId { get; set; }
        public string CityName { get; set; }
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }
        public string? CityCode { get; set; }
        public string? PinCode { get; set; }
        public string IsTehsil { get; set; }

        [JsonProperty(PropertyName = "start")]
        public int? Start { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int? Length { get; set; }

        public string? Search { get; set; }

    }

    public class StatesResponseModel
    {

        public int StateId { get; set; }
        public string? StateName { get; set; }
        public string? StateCode { get; set; }

        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }

        public string? Search { get; set; }
    }

    public class DistrictsResponseModel
    {

        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public string? DistrictCode { get; set; }
        public int StateId { get; set; }

        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }

        public string? Search { get; set; }
    }









}
