using Newtonsoft.Json;

namespace cwcerp.MDM_Models.Request
{
    public class CitiesRequestModel
    {

        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }

        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }

        public string? Search { get; set; }

    }
}
