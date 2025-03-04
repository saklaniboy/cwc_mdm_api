using cwcerp.Mdm_Models.Request;
using cwcerp.Mdm_Models.Response;

namespace cwcerp.Mdm_Service.IService
{
    public interface IBankService
    {

        Response GetBankList();    
        Response GetBankIFSC(int bankId);
    }
}
