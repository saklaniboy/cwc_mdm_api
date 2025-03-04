using System.ComponentModel.DataAnnotations;

namespace cwcerp.MDM_Models.Response
{
    public class PcsAdditionalDetailsModel
    {

        public int OfficeId { get; set; }
        public PcsDetails? PcsDetails { get; set; }
        public PcsContactDetails? PcsContactDetails { get; set; }
    }

    public class PcsDetails
    {
        public string? OpeningTime { get; set; }
        public string? ClosingTime { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }

    public class PcsContactDetails
    {
        public string? Email { get; set; }
        public string? Mobileno { get; set; }
    }
}
