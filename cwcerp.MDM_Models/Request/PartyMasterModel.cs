using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cwcerp.MDM_Models.Response
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidationErrorCodeAttribute : Attribute
    {
        public int ErrorCode { get; }

        public ValidationErrorCodeAttribute(int errorCode)
        {
            ErrorCode = errorCode;
        }
    }
    public class PartyMasterModel
    {
        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField
        public int PartyTypeId { get; set; }
        public int? PartyId { get; set; }
        public int? MainTypeId { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(128, ErrorMessage = "PartyName can't be longer than 128 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string PartyName { get; set; }

        [Required(ErrorMessage = "Required.")]
        [Phone(ErrorMessage = "Invalid.")]
        [ValidationErrorCode(2001)] // Error code for InvalidFormat
        public string PrimaryMobileNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid.")]
        [ValidationErrorCode(2002)] // Error code for InvalidFormat
        public string PrimaryEmail { get; set; }

        [StringLength(50, ErrorMessage = "PartyAccountCode can't be longer than 50 characters.")]
        [ValidationErrorCode(3001)] // Error code for MaxLengthExceeded
        public string PartyAccountCode { get; set; }

        public bool IsStorage { get; set; }
        public bool IsExportImport { get; set; }
        public bool IsCHA { get; set; }
        public bool IsUserCreated { get; set; }

        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField
        public long CreatedBy { get; set; }

        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1003)] // Error code for RequiredField (nested object)
        public List<PartyContactInfoModel> PartyContacts { get; set; }

        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1003)] // Error code for RequiredField (nested object)
        public List<PartyAddressModel> PartyAddress { get; set; }

        public List<PartyAditionalDetailModel> PartyAditionalDetail { get; set; }
        public List<PartyExportLicenseDetailModel> PartyExportLicenseDetail { get; set; }
    }

    public class PartyContactInfoModel
    {
        [Required(ErrorMessage = "ContactType is required.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField
        public int ContactType { get; set; }

        [EmailAddress(ErrorMessage = "Invalid.")]
        [ValidationErrorCode(2002)] // Error code for InvalidFormat
        public string EmailId { get; set; }

        [Phone(ErrorMessage = "Invalid.")]
        [ValidationErrorCode(2001)] // Error code for InvalidFormat
        public string ContactNumber { get; set; }

        [Phone(ErrorMessage = "Invalid.")]
        [ValidationErrorCode(2001)] // Error code for InvalidFormat
        public string Mobile { get; set; }

        [Phone(ErrorMessage = "Invalid.")]
        [ValidationErrorCode(2001)] // Error code for InvalidFormat
        public string Landline { get; set; }

        [Phone(ErrorMessage = "Invalid.")]
        [ValidationErrorCode(2001)] // Error code for InvalidFormat
        public string FaxNumber { get; set; }

        public int? LangId { get; set; }
        public int? StatusId { get; set; }

        [StringLength(200, ErrorMessage = "Designation can't be longer than 200 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string Designation { get; set; }
    }

    public class PartyAddressModel
    {
        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField
        public byte AddressTypeId { get; set; }

        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField
        public string GstNumber { get; set; }

        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField
        public string Name { get; set; }

        public string PanNumber { get; set; }
        public string TanNumber { get; set; }

        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string LatLong { get; set; }

        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }

        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "PinCode must be 6 digits.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string PinCode { get; set; }

        public short? LangId { get; set; }
        public int? StatusId { get; set; }
    }

    public class PartyExportLicenseDetailModel
    {
        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField
        public string LicenseNo { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class PartyAditionalDetailModel
    {
        [Required(ErrorMessage = "Required.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField
        public string Name { get; set; }
    }

    public class PartyStatusModel
    {
        public int PartyId { get; set; }
        public bool? IsStorage { get; set; }
        public bool? IsCHA { get; set; }
        public bool? IsUserCreated { get; set; }
    }


}