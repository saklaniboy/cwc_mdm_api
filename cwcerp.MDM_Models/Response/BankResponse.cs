using System.Security.Principal;
using System.Text.Json.Serialization;

namespace cwcerp.Mdm_Models.Request
{
    public class BankResponse
    {
        public int BankId { get; set; }                
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public int recordstatus_id { get; set; }
        public DateTime created_date { get; set; }
    }

    public class BankIFSC
    {        
        public int BankId { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string ContactNo { get; set; }
        public int recordstatus_id { get; set; }
        public DateTime created_date { get; set; }
    }
}
