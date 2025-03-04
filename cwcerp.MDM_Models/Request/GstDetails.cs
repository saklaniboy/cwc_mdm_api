using System.Security.Principal;
using System.Text.Json.Serialization;

namespace cwcerp.Mdm_Models.Request
{
    public class GstDetails
    {
        public string GstNumber { get; set; }
        public string BusinessName { get; set; }
        public string PanNumber { get; set; }
        public string TanNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string StateId { get; set; }
        public string DistrictId { get; set; }
        public string CityId { get; set; }
        public string PinCode { get; set; }
        
    }
}
