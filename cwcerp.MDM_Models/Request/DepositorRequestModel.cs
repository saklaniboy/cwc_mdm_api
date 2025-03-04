namespace cwcerp.MDM_Models.Request
{
    public class DepositorRequestModel
    {
        public int? PartyId { get; set; } = null;
        public int? PartyTypeId { get; set; } = null;
        public int? MainTypeId { get; set; } = null;
        public string? PartyName { get; set; }=string.Empty;
        public string? PrimaryMobileNumber { get; set; }= string.Empty; 
        public string? PrimaryEmail { get; set; }=string.Empty;
        public string? PartyAccountCode { get; set; } = string.Empty;
        /// <summary>
        /// Params= "1,2,3"
        /// param for IsStorage=1
        /// Param for IsExportImport=2
        /// Param for IsCHA=3
        /// </summary>
        public string? Services { get; set; } = string.Empty; 
        //public bool? IsStorage { get; set; }=null;
        //public bool? IsExportImport { get; set; }=null;
        //public bool? IsCHA { get; set; }=null;
        public bool? IsUserCreated { get; set; }=null;

    }
}
