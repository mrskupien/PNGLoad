public class ErrorLog
{
    private ErrorLog(string value) { Message = value; }
    public string Message { get; private set; }


    private const string noDirectoryMessage = "There is no such directory";
    private const string noFilesMessage = "There are no files to load";


    public static ErrorLog OK => new ErrorLog(string.Empty);
    public static ErrorLog NoDirectory => new ErrorLog(noDirectoryMessage);
    public static ErrorLog NoFiles => new ErrorLog(noFilesMessage);
}
