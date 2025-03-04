using Dapper;
using System.Data;
using cwcerp.Mdm_Models.Response;
using cwcerp.Mdm_Repository;
using System.Reflection;
using System.Linq;
using cwcerp.MDM_Models.Uitlity;
using System.Data.SqlClient;
namespace cwcerp.MDM_Service.IService
{
    public class VendorMasterService : IVendorMasterService
    {
        Response response;
        PagingResponse pagingResponse;
        private readonly IDapperConnection _dapper;
        public VendorMasterService(IDapperConnection dapper)
        {
            _dapper = dapper;
        }

        public Response UpsertVendorMaster(List<VendorMasterModel> validModels)
        {
            var response = new Response();

            if (validModels == null || !validModels.Any())
            {
                response.success = false;
                response.message = "No valid vendor records to insert.";
                response.errorCode = 1003;
                return response;
            }

            try
            {
                ListtoDataTableConverter converter = new ListtoDataTableConverter();
                List<DynamicListJson> TempDynamicArray = validModels.Select(item => new DynamicListJson()
                {
                    VendorCode = item.VendorCode,
                    VendorName = item.VendorName,
                    VendorName2 = item.VendorName2,
                    Country = item.Country,
                    Region = item.Region,
                    StreetHouse = item.StreetHouse,
                    Street2 = item.Street2,
                    Street3 = item.Street3,
                    District = item.District,
                    City = item.City,
                    PostalCode = item.PostalCode,
                    TelephoneNo = item.TelephoneNo,
                    MobileNo = item.MobileNo,
                    EmailId = item.EmailId,
                    Category = item.Category,
                    TaxNo = item.TaxNo,
                    BankKey = item.BankKey,
                    BankAccount = item.BankAccount,
                    AccountHolder = item.AccountHolder,
                    ReconcilationAccount = item.ReconcilationAccount
                }).ToList();

                DataTable AddVendorTable = converter.ToDataTable(TempDynamicArray);
                var dbparams = new DynamicParameters();
                dbparams.Add("@VendorMasterData", AddVendorTable, DbType.Object);

                var result = this._dapper.Get<Response>("USP_UpsertVendorMasterNew", dbparams, commandType: CommandType.StoredProcedure);

                response.success = result?.success ?? false;
                response.message = result?.message ?? "Unknown error.";
                response.errorCode = result?.errorCode ?? 5000;
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = "An error occurred while processing the request.";
                response.errorCode = 5000;
            }

            return response;
        }

        public Response GetVendorList(int pageNumber, int pageSize)
        {
            var response = new Response();
            try
            {
                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                dbparams.Add("@PageNumber", pageNumber, DbType.Int32);
                dbparams.Add("@PageSize", pageSize, DbType.Int32);
                // Execute the stored procedure and fetch results
                //var partys = _dapper.GetAll<dynamic>(
                //    "[master].[MDM_GetPartyList]",
                //    dbparams,
                //    commandType: CommandType.StoredProcedure);
                using (var multi = _dapper.QueryMultiple(
                       "[dbo].[MDM_GetVendorList]",
                       dbparams,
                       commandType: CommandType.StoredProcedure))
                {
                    // Fetch first result set (PartyMaster)
                    var partyMasterList = multi.Read<dynamic>().ToList();
                    // Fetch second result set (PartyAddress)
                    var partyAddressList = multi.Read<dynamic>().ToList();
                    // Fetch second result set (partyPartyContact)
                    var partyPartyContactList = multi.Read<dynamic>().ToList();
                    // Fetch second result set (partyAdditionalDetail)
                    var partyAdditionalDetailList = multi.Read<dynamic>().ToList();
                    // Fetch second result set (partyExportLicense)
                    var partyExportLicenseList = multi.Read<dynamic>().ToList();
                    // Loop through each party in the main partyMasterList
                    foreach (var party in partyMasterList)
                    {
                        var partyId = party.PartyId;
                        // Manually filter and assign the matching PartyAddress based on PartyId
                        party.PartyAddress = partyAddressList
                            .Where(x => x.PartyId == partyId)
                            .ToList();
                        // Manually filter and assign the matching PartyContact based on PartyId
                        party.PartyContact = partyPartyContactList
                            .Where(x => x.PartyId == partyId)
                            .ToList();
                        // Manually filter and assign the matching AdditionalDetail based on PartyId
                        party.AdditionalDetail = partyAdditionalDetailList
                            .Where(x => x.PartyId == partyId)
                            .ToList();
                        // Manually filter and assign the matching ExportLicense based on PartyId
                        party.ExportLicense = partyExportLicenseList
                            .Where(x => x.PartyId == partyId)
                            .ToList();
                    }
                    // Prepare the response with the updated main array
                    var result = new
                    {
                        PartyMaster = partyMasterList
                    };
                    // Set the response
                    response.success = true;
                    response.message = "Vendor list retrieved successfully.";
                    response.responseData = result;
                }
                // Set the response
                //response.success = true;
                //response.message = "Party list retrieved successfully.";
                //response.responseData = partys;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while retrieving the party list.";
                response.responseData = ex.Message;
            }
            return response;
        }

    }
}