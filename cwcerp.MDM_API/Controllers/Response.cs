namespace cwcerp.MDM_API.Controllers
{
    internal class Response<T>
    {
        private bool v1;
        private string v2;
        private object value;

        public Response(bool v1, string v2, object value)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.value = value;
        }
    }
}