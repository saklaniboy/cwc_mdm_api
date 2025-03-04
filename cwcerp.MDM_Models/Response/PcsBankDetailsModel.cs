using System.ComponentModel.DataAnnotations;

namespace cwcerp.MDM_Models.Response
{
    public class PcsBankDetailsModel
    {

        public int OfficeId { get; set; }

        public int PrimaryAccountNo { get; set; }

        public string AccountHolderName { get; set; }

        public string PrimaryIFSCCode { get; set; }

        public string Branch { get; set; }

        public string Address { get; set; }

        public int CreatedBy { get; set; }
    }
}
