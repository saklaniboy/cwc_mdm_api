namespace cwcerp.MDM_Models.Response
{
    public class PcsBasicInfoModel
    {
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int? OfficeTypeId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string PinCode { get; set; }
        public DateTime? ConstructedDate { get; set; } 
        public int? OfficeId { get; set; } 
        public string GstNumber { get; set; } 
        public int? OfficeParentId { get; set; } 
        public int? AddressTypeId { get; set; }
        public int? created_userid { get; set; }
    }



}

