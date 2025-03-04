namespace cwcerp.MDM_Models.Response
{
    public class PcsAccountDetails
    {
        public int OfficeId { get; set; }
        public string? LedgerCode { get; set; }
        public string? CostCentreCode { get; set; }
        public string? ProfitCenterCode { get; set; }
        public string? PlantCode { get; set; }
        public int? created_userid { get; set; }
    }
}
