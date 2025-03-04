public class ValidationErrorCodeAttribute : Attribute
{
    public int ErrorCode { get; }
    public ValidationErrorCodeAttribute(int errorCode)
    {
        ErrorCode = errorCode;
    }
}
