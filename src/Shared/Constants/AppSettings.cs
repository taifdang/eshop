namespace Shared.Constants;

public class AppSettings
{
    public Identity Identity { get; set; }
    public FileStorageSettings FileStorageSettings { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public MailConfig MailConfig { get; set; }
    public string BaseURL { get; set; } = "";
}
public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}
public class FileStorageSettings
{
    public bool LocalStorage { get; set; } = true;
    public string Path { get; set; } = "";
}
public class MailConfig 
{
    public string From { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}
public class Identity
{   
    public string Key { get; set; }
    public string Authority { get; set; }
    public string Audience { get; set; }
    public int ExpiredTime { get; set; }
}

public class BackgroundTaskOptions
{
    public int GracePeriodTime { get; set; }
    public int CheckUpdateTime { get; set; }
}
