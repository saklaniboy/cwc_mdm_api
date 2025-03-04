using cwcerp.Mdm_Models.Response;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace cwcerp.Mdm_Models.Request
{
    public class SatelliteOfficeResponse
    {
        public OfficeStatus? OfficeStatus { get; set; }
        public SoBasicInfo? OfficeDetails { get; set; }
        public SoBillingInfo? OfficeAddress { get; set; }
        public SoBankInfo? OfficeSourceBank { get; set; }
        public SoContactInfo? OfficeContactDetails { get; set; }
    }

    public class SatelliteOfficeList
    {
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string OfficeCode { get; set; }
        public int ApprovalStatus { get; set; }
        public int StatusId { get; set; }
        public DateTime created_date { get; set; }
        public int RequestID { get; set; }
    }

    public class SatelliteStatus
    {
        public int OfficeId { get; set; }
        public int OfficeTypeId { get; set; }
        public int ProcessId { get; set; }
        public int StatusId { get; set; }
        public int StepId { get; set; }
    }



}
