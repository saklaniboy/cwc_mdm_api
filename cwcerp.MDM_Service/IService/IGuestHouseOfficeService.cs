using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;

namespace cwcerp.Mdm_Service.IService
{
    public interface IGuestHouseOfficeService
    {
        Response GetGhList(int pageNumber, int pageSize);
        Response GetGhInfo(int officeId);
        Response BasicInfo(GhBasicInfo request);
        Response BillingAddressInfo(GhBillingInfo request);
        Response BankInfo(GhBankInfo request);
        Response GetGhDetails(int officeId);
        Response ContactInfo(GhContactInfo request);
        Response OfficeStatus(OfficeStatus request);
    }
}
