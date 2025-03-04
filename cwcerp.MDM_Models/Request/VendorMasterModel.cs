using System.ComponentModel.DataAnnotations;

namespace cwcerp.Mdm_Models.Response
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
    public class VendorMasterModel
    {
        [Required(ErrorMessage = "Required.")]
        [StringLength(10, ErrorMessage = "VendorNumber must not exceed 10 characters.")]
        [ValidationErrorCode(1002)] // Error code for RequiredField/MaxLengthExceeded
        public string VendorCode { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(40, ErrorMessage = "VendorName must not exceed 40 characters.")]
        [ValidationErrorCode(1002)] // Error code for RequiredField/MaxLengthExceeded
        public string VendorName { get; set; }

        [StringLength(40, ErrorMessage = "VendorName2 must not exceed 40 characters.")]
        [ValidationErrorCode(1002)] // Error code for RequiredField/MaxLengthExceeded
        public string? VendorName2 { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(2, ErrorMessage = "Country must not exceed 2 characters.")]
        [ValidationErrorCode(1002)] // Error code for RequiredField/MaxLengthExceeded
        public string Country { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(3, ErrorMessage = "Region must not exceed 2 characters.")]
        [ValidationErrorCode(1001)] // Error code for RequiredField/MaxLengthExceeded
        public string Region { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(40, ErrorMessage = "StreetHouse must not exceed 40 characters.")]
        [ValidationErrorCode(1002)] // Error code for RequiredField/MaxLengthExceeded
        public string StreetHouse { get; set; }

        [StringLength(40, ErrorMessage = "Street2 must not exceed 40 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? Street2 { get; set; }

        [StringLength(40, ErrorMessage = "Street3 must not exceed 40 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? Street3 { get; set; }

        [StringLength(40, ErrorMessage = "District must not exceed 40 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? District { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(40, ErrorMessage = "City must not exceed 40 characters.")]
        [ValidationErrorCode(1002)] // Error code for RequiredField/MaxLengthExceeded
        public string? City { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "PostalCode must be 6 digits.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string PostalCode { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Telephone number must be exactly 10 digits.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? TelephoneNo { get; set; }

        [StringLength(15, MinimumLength = 10, ErrorMessage = "Mobile number must be between 10 and 15 digits.")]
        [RegularExpression(@"^\+?[1-9][0-9]{9,14}$", ErrorMessage = "Invalid mobile number format.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? MobileNo { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(241, ErrorMessage = "Email cannot exceed 255 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? EmailId { get; set; }

        [StringLength(4, ErrorMessage = "Category must not exceed 4 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? Category { get; set; }

        [StringLength(20, ErrorMessage = "Tax Number must not exceed 20 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? TaxNo { get; set; }

        [StringLength(15, ErrorMessage = "Bank Key must not exceed 15 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? BankKey { get; set; }

        [StringLength(18, ErrorMessage = "Bank Account must not exceed 18 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? BankAccount { get; set; }

        [StringLength(60, ErrorMessage = "Account Holder must not exceed 60 characters.")]
        [ValidationErrorCode(1002)] // Error code for MaxLengthExceeded
        public string? AccountHolder { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(10, ErrorMessage = "Reconcilation Account must not exceed 10 characters.")]
        [ValidationErrorCode(1002)] // Error code for RequiredField/MaxLengthExceeded
        public string ReconcilationAccount { get; set; }        

    }
    public class DynamicListJson
    {
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string? VendorName2 { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string StreetHouse { get; set; }
        public string? Street2 { get; set; }
        public string? Street3 { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string PostalCode { get; set; }
        public string? TelephoneNo { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public string? Category { get; set; }
        public string? TaxNo { get; set; }
        public string? BankKey { get; set; }
        public string? BankAccount { get; set; }
        public string? AccountHolder { get; set; }
        public string ReconcilationAccount { get; set; }
    }
}