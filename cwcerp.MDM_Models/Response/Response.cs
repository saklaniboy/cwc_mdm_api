namespace cwcerp.Mdm_Models.Response
{
    public class Response : IDisposable
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object responseData { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public int? errorCode { get; set; } // Add errorCode property 

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}