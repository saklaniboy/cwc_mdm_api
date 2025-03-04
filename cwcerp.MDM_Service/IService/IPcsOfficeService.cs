using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Response;

namespace cwcerp.MDM_Service.IService
{
    public interface IPcsOfficeService
    {
        Response AddPcsBasicInfo(PcsBasicInfoModel pcsOffice);
        Response AddOrUpdatePcsAccountDetails(PcsAccountDetails pcsOffice);
        Response AddPcsBankDetails(PcsBankDetailsModel pcsOffice);
        Response AddPcsAdditionalDetails(PcsAdditionalDetailsModel pcsOffice);
        Response GetPcsList(int? pageNumber, int? pageSize);
        Response UpdateOfficeStatus(WOfficeStatus pcsOffice);
        Response GetPcsDetailsById(int officeId);

    }
}
