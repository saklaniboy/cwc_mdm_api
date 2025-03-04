using cwcerp.Mdm_Models.Response;

namespace cwcerp.MDM_API
{
    public static class ApiResponse
    {
        public static object CreateSuccessResponse(object data, int recordsTotal = 0, string message = "Success")
        {
            return new
            {
                Message = message,
                Success = true,
                ResponseData = data,
                RecordsTotal = recordsTotal
            };
        }

        public static object CreateErrorResponse(string message, object data = null, int recordsTotal = 0)
        {
            return new
            {
                Message = message,
                Success = false,
                ResponseData = data,
                RecordsTotal = recordsTotal
            };
        }
    }
}
