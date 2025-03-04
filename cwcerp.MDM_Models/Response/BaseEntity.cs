using System;

namespace cwcerp.Mdm_Models.Response
{
    public class BaseEntity
    {           
        public int StatusId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}