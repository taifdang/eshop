namespace Shared.Models.Auth;

public class TokenResult
{
    public string Token { get; set; }
    public DateTime Expire {  get; set; }
}
