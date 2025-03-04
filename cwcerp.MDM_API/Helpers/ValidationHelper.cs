
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

public static class ValidationHelper
{
    public static List<ValidationError> ValidateModel(object model)
    {
        var errors = new List<ValidationError>();

        if (model == null)
        {
            errors.Add(new ValidationError
            {
                Code = 1000, // Default error code for null models
                FieldName = "Model",
                Message = "Model cannot be null."
            });
            return errors;
        }

        // Validate the main object
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(model, validationContext, validationResults, true);

        foreach (var validationResult in validationResults)
        {
            foreach (var memberName in validationResult.MemberNames)
            {
                var property = model.GetType().GetProperty(memberName);

                if (property != null)
                {
                    // Retrieve the custom validation error code
                    var errorCode = GetValidationErrorCode(property, validationResult);

                    errors.Add(new ValidationError
                    {
                        Code = errorCode,
                        FieldName = memberName,
                        Message = validationResult.ErrorMessage
                    });
                }
            }
        }

        // Now handle nested collections like PartyContacts, PartyAddress, etc.
        var collectionProperties = model.GetType().GetProperties()
            .Where(p => p.PropertyType.IsGenericType && typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType));

        foreach (var collectionProperty in collectionProperties)
        {
            var collection = collectionProperty.GetValue(model) as System.Collections.IEnumerable;

            // Handle empty arrays by checking if they are required
            if (collection != null && !collection.Cast<object>().Any())
            {
                var propertyName = collectionProperty.Name;

                // Check if the collection is required and if empty, add an error
                var requiredAttribute = collectionProperty.GetCustomAttribute<RequiredAttribute>();
                if (requiredAttribute != null)
                {
                    errors.Add(new ValidationError
                    {
                        Code = 1001, // Custom error code for empty required collections
                        FieldName = propertyName,
                        Message = "Required"
                        //Message = $"{propertyName} cannot be empty."
                    });
                }
            }

            // Validate each item in the collection (nested objects)
            var index = 0;
            foreach (var item in collection)
            {
                if (item != null)
                {
                    // Update FieldName to include index for sub-array items
                    var fieldNameWithIndex = $"{collectionProperty.Name}[{index}]";

                    // Validate sub-models and apply custom error codes
                    errors.AddRange(ValidateModel(item).Select(err =>
                        new ValidationError
                        {
                            Code = err.Code,
                            FieldName = $"{fieldNameWithIndex}.{err.FieldName}",
                            Message = err.Message
                        }));
                    index++;
                }
            }
        }

        return errors;
    }

    private static int GetValidationErrorCode(PropertyInfo property, ValidationResult validationResult)
    {
        // Default error code if none is provided
        var defaultErrorCode = 9999;

        // Check if the property has a ValidationErrorCodeAttribute
        var errorCodeAttribute = property.GetCustomAttributes(typeof(ValidationErrorCodeAttribute), false)
            .Cast<ValidationErrorCodeAttribute>()
            .FirstOrDefault();

        if (errorCodeAttribute != null)
        {
            return errorCodeAttribute.ErrorCode;
        }

        // Map specific validation types to custom error codes
        if (validationResult.ErrorMessage.Contains("Required"))
        {
            return 1001; // RequiredField error code
        }
        
        if (validationResult.ErrorMessage.Contains("Invalid"))
        {
            return 1002; // Invalid phone number format
        }
        //if (validationResult.ErrorMessage.Contains("can't be longer than"))
        //{
        //    return 1003; // MaxLengthExceeded error code
        //}
        //if (validationResult.ErrorMessage.Contains("Invalid"))
        //{
        //    return 2002; // Invalid email format
        //}

        // Return default error code if no match
        return defaultErrorCode;
    }
    public class ValidationError
    {
        public int Code { get; set; }
        public string FieldName { get; set; }
        public string Message { get; set; }
    }


    public static List<ValidationError> ValidateModelVendors(object model)
    {
        var errors = new List<ValidationError>();

        if (model == null)
        {
            errors.Add(new ValidationError
            {
                Code = 1000,
                FieldName = "Model",
                Message = "Model cannot be null."
            });
            return errors;
        }

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(model, validationContext, validationResults, true);

        foreach (var validationResult in validationResults)
        {
            foreach (var memberName in validationResult.MemberNames)
            {
                var property = model.GetType().GetProperty(memberName);
                var errorCode = GetValidationErrorCode(property, validationResult);

                errors.Add(new ValidationError
                {
                    Code = errorCode,
                    FieldName = memberName,
                    Message = validationResult.ErrorMessage
                });
            }
        }

        // Validate collections (nested objects)
        var collectionProperties = model.GetType().GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                        typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType));

        foreach (var collectionProperty in collectionProperties)
        {
            var collection = collectionProperty.GetValue(model) as System.Collections.IEnumerable;
            if (collection != null)
            {
                int index = 0;
                foreach (var item in collection)
                {
                    if (item != null)
                    {
                        errors.AddRange(ValidateModelVendors(item).Select(err =>
                            new ValidationError
                            {
                                Code = err.Code,
                                FieldName = $"{collectionProperty.Name}[{index}].{err.FieldName}",
                                Message = err.Message
                            }));
                    }
                    index++;
                }
            }
        }

        return errors;
    }

}
