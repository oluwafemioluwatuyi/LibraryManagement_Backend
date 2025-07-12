namespace LibraryManagement.Constants
{
    public enum AppStatusCodes
    {
        Success = 0000,
        EmailNotVerified = 0001,
        InvalidCredentials = 0003,
        InvalidVerificationToken = 0004,
        AlreadyExists = 0006,
        ResourceNotFound = 0007,
        Unauthorized = 0008,
        InvalidData = 0009,
        InternalServerError = 9999,
    }
}
