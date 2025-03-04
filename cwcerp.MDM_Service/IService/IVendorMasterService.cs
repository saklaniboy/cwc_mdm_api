using cwcerp.Mdm_Models.Response;
namespace cwcerp.MDM_Service.IService
{
    public interface IVendorMasterService
    {
        Response UpsertVendorMaster(List<VendorMasterModel> vendorMaster);
        Response GetVendorList(int pageNumber, int pageSize);
    }
}