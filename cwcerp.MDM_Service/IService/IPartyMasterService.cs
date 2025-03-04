using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Request;
using cwcerp.MDM_Models.Response;
namespace cwcerp.MDM_Service.IService
{
    public interface IPartyMasterService
    {
        Response AddPartyMaster(List<PartyMasterModel> partyMaster);
        Response GetPartyList(int pageNumber, int pageSize);
        Response GetDipositorList(DepositorRequestModel model);
        Response UpdatePartyStatus(PartyStatusModel model);
        Response GetDipositorList(int DepositorMainType = 0, int DipositorSubType = 0, string TypeOfServices = "", string searchText = "");

    }
}