namespace cwcerp.MDM_Models.Response
{
    public class WarehouseAreaDetailsModel
    {
        public int OfficeId { get; set; }
        public decimal OpenArea { get; set; }
        public decimal CoveredArea { get; set; }
        public decimal DistanceFromRoInKm { get; set; }
        public string? EarthquakeZone { get; set; }
        public int  IsBuffer { get; set; }
        public int CreatedBy { get; set; }
    }
}
