using System.Security.Principal;
using System.Text.Json.Serialization;

namespace cwcerp.Mdm_Models.Request
{
    public class RegionOfficeRequest
    {
        //
    }

    public class RoBasicInfo
    {
        public int? OfficeId { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int OfficeTypeId { get; set; }
        public int? OfficeParentId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AddressTypeId { get; set; }
        public string? AddressLine1 { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public string? PinCode { get; set; }    
    }

    public class RoBillingInfo
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

    public class RoBankInfo
    {
        public int officeId { get; set; }  
        public string? primaryBankId { get; set; }
        public string primaryAccountNumber { get; set; }
        public string? primaryBranchId { get; set; }
        public string? primaryIfscCode { get; set; }  
        //public bool? secondaryAccountEnabled { get; set; }
        public string? secondaryBankId { get; set; }
        public string? secondaryAccountNumber { get; set; }
        public string? secondaryBranchId { get; set; }
        public string? secondaryIfscCode { get; set; }    
    }

    public class Accounts
    {
        public int BankId { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }      
        public int? IFSCId { get; set; }
        public int? IsPrimary { get; set; }
    }

    public class RoContactInfo
    {
        public int OfficeId { get; set; }
        public string EmailId { get; set; }
        public string ContactNumber { get; set; }
        public string? Landline { get; set; }
        public string? FaxNumber { get; set; }        
        public string? IPAddress { get; set; }        
    }


}
