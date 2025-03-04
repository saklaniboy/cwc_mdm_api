using System.ComponentModel.DataAnnotations;

namespace cwcerp.MDM_Models.Response
{
    public class WarehouseBankDetailsModel
    {

        public int officeId { get; set; }

        public Int64 PrimaryAccountNo { get; set; }

        public string PrimaryIFSCCode { get; set; } 

        public string? Branch { get; set; }

        public string? Address { get; set; }

        public int? CreatedBy { get; set; }
        public int? pageId { get; set; }
        public int? sectionId { get; set; }
    }
}
