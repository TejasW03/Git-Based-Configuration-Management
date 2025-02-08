public class GitServiceException : Exception
{
    public GitServiceException(string message) : base(message) { }
    public GitServiceException(string message, Exception innerException) 
        : base(message, innerException) { }
}