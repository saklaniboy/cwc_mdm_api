using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;

namespace cwcerp.Mdm_Service.IService
{
    public interface IRegionOfficeService
    {
        Response GetRoList(int? pageNumber, int? pageSize, int? statusId);
        Response GetRoInfo(int officeId);
        Response BasicInfo(RoBasicInfo request);
        Response BillingAddressInfo(RoBillingInfo request);
        Response BankInfo(RoBankInfo request);
        Response ContactInfo(RoContactInfo request);
        Response OfficeStatus(OfficeStatus request);
    }
}
