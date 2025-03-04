using cwcerp.Mdm_Models.Response;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace cwcerp.Mdm_Models.Request
{
    public class GuestHouseOfficeResponse
    {
        public OfficeStatus? OfficeStatus { get; set; }
        public GhBasicInfo? OfficeDetails { get; set; }
        public GhBillingInfo? OfficeAddress { get; set; }
        public GhBankInfo? OfficeSourceBank { get; set; }
        public GhContactInfo? OfficeContactDetails { get; set; }
    }

    public class GuestHouseOfficeList
    {
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string OfficeCode { get; set; }
        public int ApprovalStatus { get; set; }
        public int StatusId { get; set; }
        public DateTime created_date { get; set; }
    }

    public class GuestHouseStatus
    {
        public int OfficeId { get; set; }
        public int OfficeTypeId { get; set; }
        public int ProcessId { get; set; }
        public int StatusId { get; set; }
        public int StepId { get; set; }
    }



}
