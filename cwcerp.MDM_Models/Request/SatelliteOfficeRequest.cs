using System.Security.Principal;
using System.Text.Json.Serialization;

namespace cwcerp.Mdm_Models.Request
{
    public class SatelliteOfficeRequest
    {
        //
    }

    public class SoBasicInfo
    {   
        public int? OfficeId { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int OfficeTypeId { get; set; }
        public int? OfficeParentId { get; set; }
        public int? OfficeCcId { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AddressTypeId { get; set; }
        public string? AddressLine1 { get; set; }
        public int StateId { get; set; }
        public int? ZoneId { get; set; }
        public int? regionid { get; set; }
        public int? TesileID { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public string? PinCode { get; set; }    
    }


    public class SoBillingInfo
    {
        public int OfficeId { get; set; }
        public int AddressTypeId { get; set; }
        public string? GstNumber { get; set; }
        public string? BusinessName { get; set; }
        public string? PanNumber { get; set; }
        public string? TanNumber { get; set; }
        public string? AddressLine1 { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public string? PinCode { get; set; }
    }

    public class SoBankInfo
    { 
        public int OfficeId { get; set; }  
        public List<BankDetails1> Accounts { get; set; }

    }

    public class BankDetails1
    {
        public int OfficeId { get; set; }
        public string? PrimaryAccountNumber { get; set; }
        public string? PrimaryIFSCCode { get; set; }
        public int? Priority { get; set; }
    }


    public class SoContactInfo
    {
        public int OfficeId { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string? LandlineNo { get; set; }
        public string? FaxNumber { get; set; }        
        public string? IPAddress { get; set; }        
    }


}
