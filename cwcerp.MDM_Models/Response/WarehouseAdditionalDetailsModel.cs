using System.ComponentModel.DataAnnotations;

namespace cwcerp.MDM_Models.Response
{
    public class WarehouseAdditionalDetailsModel
    {

        public int OfficeId { get; set; }
        public OfficesDetails? OfficesDetails { get; set; }
        public InfrastructureDetails? InfrastructureDetails { get; set; }
        public ContactDetails? ContactDetails { get; set; }


    }

    public class OfficesDetails
    {
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
    
    public class OfficeCustomModel
    {
        public int officeId { get; set; } 
    }


        public class InfrastructureDetails
    {
        public string? OpeningTime { get; set; }
        public string? ClosingTime { get; set; }
        public int Gates { get; set; }
        public string? RegistrationNo { get; set; }
        public string? WarehouseCategoryID { get; set; }
        public string? Weighbridge { get; set; }
        public string? IsMoistureMeter { get; set; }
        public string WeighbridgesNo { get; set; }
        public string IsLorryWeighbridge { get; set; }
        public string WDRARegistrered { get; set; }
        public string? Licencesdetails { get; set; }
    }

    public class ContactDetails
    {
        public string? Email { get; set; }
        public string? Mobileno { get; set; }
    }

    public class TokenData
    {
        public Int64 UserId { get; set; }
        public Int64? CompanyId { get; set; }
        public Int32? ModuleId { get; set; }
    }
}
