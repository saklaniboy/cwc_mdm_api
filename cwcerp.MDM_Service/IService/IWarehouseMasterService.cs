using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Response;

namespace cwcerp.MDM_Service.IService
{
    public interface IWarehouseMasterService
    {

        //Response GetWarehouseOfficeList();

        //Response UpdateWarehouseOffice(int locationid, OfficeInfoModel NewWarehouse);
        //Response DeleteWarehouseOffice(int locationid);

        Response AddWarehouseMaster(WarehouseMasterModel warehouseMaster);
        Response AddOrUpdateWarehouseAreaDetails(WarehouseAreaDetailsModel model);
        Response AddOrUpdateWarehouseAccountDetails(WarehouseAccountDetailsModel model);
        Response AddWarehouseBankDetails(WarehouseBankDetailsModel model);
        Response AddWarehouseAdditionalDetails(WarehouseAdditionalDetailsModel model);
        Response GetWarehouseList(int pageNumber, int pageSize, int? locationId = null);
        Response UpdateOfficeStatus(WOfficeStatus model);
        Response GetWarehouseDetailsById(int officeId);
        Response GetWarehouseCategories();
        Response GetWarehouseOwner();
        Response GetWarehouseTypes();


        //List<OfficeInfoModel> GetWarehouseOfficeList();
        //List<OfficeInfoModel> AddWarehouseOffice(OfficeInfoModel warehouseOffice);
        //List<OfficeInfoModel> UpdateWarehouseOffice(int locationid, OfficeInfoModel NewWarehouse);
        //List<OfficeInfoModel> DeleteWarehouseOffice(int locationid);




    }
}
