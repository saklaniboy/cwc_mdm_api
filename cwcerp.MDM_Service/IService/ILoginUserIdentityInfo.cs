using Microsoft.AspNetCore.Http;

namespace cwcerp.Mdm_Service.IService
{
    public interface ILoginUserIdentityInfo
    {
        int LoginedUserId(HttpContext httpContext);
    }
}
