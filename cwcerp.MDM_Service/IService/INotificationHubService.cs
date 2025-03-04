using System.Threading.Tasks;

namespace cwcerp.Mdm_Service.IService
{
    public interface INotificationHubService
    {
        Task GetLocation(string lat, string lon);
        Task DepartmentActivity(string message);
        Task NewNotification(string message);
    }
}
