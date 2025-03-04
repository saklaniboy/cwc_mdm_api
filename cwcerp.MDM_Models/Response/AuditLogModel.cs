namespace cwcerp.MDM_Models.Response
{
    public class AuditLogModel
    {
        public int LogID { get; set; }
        public string? TableName { get; set; }
        public string? OperationType { get; set; }
        public int RecordID { get; set; }
        public string? OldData { get; set; }
        public string? NewData { get; set; }
        public string? IPAddress { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
