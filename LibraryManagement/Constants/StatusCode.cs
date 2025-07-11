namespace LibraryManagement.Constants
{
    public enum AppStatusCodes
    {
        Success = 0000,
        BvnNotVerified = 0001,
        EmailNotVerified = 0002,
        InvalidCredentials = 0004,
        InvalidVerificationToken = 0005,
        AlreadyExists = 0006,
        ResourceNotFound = 007,
        Unauthorized = 0008,
        InvalidData = 0009,
        InternalServerError = 9999,
    }
}
