using cwcerp.MDM_Service.IService;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using cwcerp.Mdm_Models.Response;
using cwcerp.MDM_Models.Response;
using cwcerp.Mdm_Repository;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using cwcerp.MDM_Models.Request;
namespace cwcerp.MDM_Service.Service
{
    public class PartyMasterService : IPartyMasterService
    {
        Response response;
        PagingResponse pagingResponse;
        private readonly IDapperConnection _dapper;
        public PartyMasterService(IDapperConnection dapper)
        {
            _dapper = dapper;
        }
        public Response AddPartyMaster(List<PartyMasterModel> models)
        {
            var response = new Response();
            try
            {
                foreach (var model in models)
                {
                    // Prepare PartyContactInfo array
                    var contactsArray = new List<object>();
                    foreach (var contact in model.PartyContacts)
                    {
                        contactsArray.Add(new
                        {
                            //contact.PartyId,
                            contact.ContactType,
                            contact.EmailId,
                            contact.ContactNumber,
                            contact.Mobile,
                            contact.Landline,
                            contact.FaxNumber,
                            contact.LangId,
                            contact.StatusId,
                            contact.Designation,
                            //contact.CreatedBy
                        });
                    }
                    // Prepare PartyAddress array
                    var addressArray = new List<object>();
                    foreach (var address in model.PartyAddress)
                    {
                        addressArray.Add(new
                        {
                            address.AddressTypeId,
                            //address.PartyId,
                            address.GstNumber,
                            address.Name,
                            address.PanNumber,
                            address.TanNumber,
                            address.AddressLine1,
                            address.AddressLine2,
                            address.AddressLine3,
                            address.LatLong,
                            address.CityId,
                            address.DistrictId,
                            address.StateId,
                            address.CountryId,
                            address.PinCode,
                            address.LangId,
                            address.StatusId,
                            //address.CreatedBy
                        });
                    }
                    // Prepare PartyAditionalDetail array
                    var aditionalDetailArray = new List<object>();
                    foreach (var detail in model.PartyAditionalDetail)
                    {
                        aditionalDetailArray.Add(new
                        {
                            //detail.PartyId,
                            detail.Name,
                            //detail.CreatedBy
                        });
                    }
                    // Prepare PartyExportLicenseDetail array
                    var exportLicenseArray = new List<object>();
                    foreach (var license in model.PartyExportLicenseDetail)
                    {
                        exportLicenseArray.Add(new
                        {
                            //license.PartyId,
                            license.LicenseNo,
                            license.StartDate,
                            license.EndDate,
                            //license.CreatedBy
                        });
                    }
                    // Prepare parameters for PartyMaster
                    var dbparams = new DynamicParameters();
                    dbparams.Add("@PartyTypeId", model.PartyTypeId, DbType.Int32);
                    dbparams.Add("@MainTypeId", model.MainTypeId, DbType.Int32);
                    dbparams.Add("@PartyName", model.PartyName, DbType.String);
                    dbparams.Add("@PrimaryMobileNumber", model.PrimaryMobileNumber, DbType.String);
                    dbparams.Add("@PrimaryEmail", model.PrimaryEmail, DbType.String);
                    //dbparams.Add("@GST", model.GST, DbType.String);
                    //dbparams.Add("@TAN", model.TAN, DbType.String);
                    //dbparams.Add("@PAN", model.PAN, DbType.String);
                    dbparams.Add("@PartyAccountCode", model.PartyAccountCode, DbType.String);
                    dbparams.Add("@IsStorage", model.IsStorage, DbType.Boolean);
                    dbparams.Add("@IsExportImport", model.IsExportImport, DbType.Boolean);
                    dbparams.Add("@IsCHA", model.IsCHA, DbType.Boolean);
                    //dbparams.Add("@IsUserCreated", model.IsUserCreated, DbType.Boolean);
                    //dbparams.Add("@CreatedBy", model.CreatedBy, DbType.Int64);
                    // Serialize JSON data for nested models
                    string contactInfoJson = JsonConvert.SerializeObject(contactsArray);
                    dbparams.Add("@ContactInfoJson", contactInfoJson, DbType.String);
                    string addressInfoJson = JsonConvert.SerializeObject(addressArray);
                    dbparams.Add("@AddressInfoJson", addressInfoJson, DbType.String);
                    string aditionalDetailJson = JsonConvert.SerializeObject(aditionalDetailArray);
                    dbparams.Add("@AditionalDetailJson", aditionalDetailJson, DbType.String);
                    string exportLicenseJson = JsonConvert.SerializeObject(exportLicenseArray);
                    dbparams.Add("@ExportLicenseJson", exportLicenseJson, DbType.String);
                    // Execute stored procedure
                    var result = this._dapper.Insert<string>("MDM_InsertPartyMasterWithDetails", dbparams, commandType: CommandType.StoredProcedure);
                    // Check result for each model
                    if (result != "Success")
                    {
                        response.success = false;
                        response.message = $"An error occurred for : {result}";
                        return response;
                    }
                }
                // If all operations are successful
                response.success = true;
                response.message = "All Party records processed successfully.";
            }
            catch (Exception e)
            {
                response.responseData = e;
                response.success = false;
                response.message = "An error occurred while processing the request.";
            }
            return response;
        }
        public Response GetPartyList(int pageNumber, int pageSize)
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
                       "[dbo].[MDM_GetPartyList]",
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
                    response.message = "Party list retrieved successfully.";
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

        public Response GetDipositorList(int DepositorMainType = 0, int DipositorSubType = 0, string TypeOfServices = "", string searchText = "")
        {
            var response = new Response();
            try
            {
                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                dbparams.Add("@PartyMainTypeId", DepositorMainType, DbType.Int32);
                dbparams.Add("@PartyTypeId", DipositorSubType, DbType.Int32);
                dbparams.Add("@Services", TypeOfServices, DbType.String);
                dbparams.Add("@SearchText", searchText, DbType.String);

                var partyMasterList = _dapper.GetAll<dynamic>(
                    "[dbo].[GetDepositorList]",
                    dbparams,
                    commandType: CommandType.StoredProcedure);
                // Prepare the response with the updated main array

                response.success = true;
                response.message = "Party list retrieved successfully.";
                response.responseData = partyMasterList;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while retrieving the party list.";
                response.responseData = ex.Message;
            }
            return response;
        }

        public Response GetDipositorList(DepositorRequestModel model)
        {
            var response = new Response();
            try
            {
                // Prepare parameters for the stored procedure
                var dbparams = new DynamicParameters();
                dbparams.Add("@PartyId", model.PartyId, DbType.Int32);
                dbparams.Add("@@MainTypeId", model.MainTypeId, DbType.Int32);
                dbparams.Add("@PartyTypeId", model.PartyTypeId, DbType.Int32);
                dbparams.Add("@Services", model.Services, DbType.String);
                dbparams.Add("@PartyName", model.PartyName, DbType.String);
                dbparams.Add("@PrimaryMobileNumber", model.PrimaryMobileNumber, DbType.String);
                dbparams.Add("@PrimaryEmail", model.PrimaryEmail, DbType.String);
                dbparams.Add("@PartyAccountCode", model.PartyAccountCode, DbType.String);
                dbparams.Add("@IsUserCreated", model.IsUserCreated, DbType.Boolean);

                var partyMasterList = _dapper.GetAll<dynamic>(
                    "[dbo].[usp_GetPartyMasterData]",
                    dbparams,
                    commandType: CommandType.StoredProcedure);
                // Prepare the response with the updated main array

                response.success = true;
                response.message = "Party list retrieved successfully.";
                response.responseData = partyMasterList;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "An error occurred while retrieving the party list.";
                response.responseData = ex.Message;
            }
            return response;
        }

        public Response AddParty(PartyMasterModel partyMaster)
        {
            throw new NotImplementedException();
        }

        public Response UpdatePartyStatus(PartyStatusModel model)
        {
            using (Response response = new Response())
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("@PartyId", model.PartyId, DbType.Int32);
               
                if (model.IsStorage.HasValue)
                    dbparams.Add("@IsStorage", model.IsStorage.Value, DbType.Boolean);
                if (model.IsCHA.HasValue)
                    dbparams.Add("@IsCHA", model.IsCHA.Value, DbType.Boolean);
                if (model.IsUserCreated.HasValue)
                    dbparams.Add("@IsUserCreated", model.IsUserCreated.Value, DbType.Boolean);
                
                int result = _dapper.Insert<int>("[dbo].[UpdatePartyStatus]", dbparams, commandType: CommandType.StoredProcedure);

                if (result > 0)
                {
                    response.message = "Success";
                    response.success = true;
                    response.responseData = result;
                }
                else
                {
                    response.message = "Failed to update party status.";
                    response.success = false;
                    response.responseData = null;
                }

                return response;
            }
        }



    }
}