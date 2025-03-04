using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;

namespace cwcerp.Mdm_Service.IService
{
    public interface ISatelliteOfficeService
    {
        Response GetSoList(int pageNumber, int pageSize);
        Response GetSoInfo(int officeId);
        Response BasicInfo(SoBasicInfo request);
        Response BillingAddressInfo(SoBillingInfo request);
        Response BankInfo(SoBankInfo request);
        Response GetSoDetails(int officeId);
        Response ContactInfo(SoContactInfo request);
        Response OfficeStatus(OfficeStatus request);
    }
}
