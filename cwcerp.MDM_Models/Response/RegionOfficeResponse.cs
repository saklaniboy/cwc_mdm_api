using cwcerp.Mdm_Models.Response;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace cwcerp.Mdm_Models.Request
{
    public class RegionOfficeResponse
    {
        public OfficeStatus? OfficeStatus { get; set; }
        public RoBasicInfo? OfficeDetails { get; set; }
        public RoBillingInfo? OfficeAddress { get; set; }
        public RoBankInfo? OfficeBank { get; set; }
        public RoContactInfo? OfficeContactDetails { get; set; }
    }

    public class RegionOfficeList
    {
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string OfficeCode { get; set; }        
        public int recordstatus_id { get; set; }
        public DateTime created_date { get; set; }
        public int ProcessId { get; set; }
        public int StepStatus { get; set; }
        public int StepId { get; set; }        
        public int RequestID { get; set; }        
    }

    public class OfficeStatus
    {
        public int OfficeId { get; set; }
        public int OfficeTypeId { get; set; }
        public int ProcessId { get; set; }
        public int StatusId { get; set; }
        public string? Reason { get; set; }
        public int StepId { get; set; }
        public int? ActionBy { get; set; }
    }

}
