using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.ExternalServices.Payment;

public class HmacHelper
{
    public static string ComputeHmacSha512(
       string secretKey,
       string rawData)
    {
        using var hmac = new HMACSHA512(
            Encoding.UTF8.GetBytes(secretKey));

        var hashBytes = hmac.ComputeHash(
            Encoding.UTF8.GetBytes(rawData));

        return Convert
            .ToHexString(hashBytes)
            .ToLower();
    }

    public static bool VerifyHmacSha512(
        string secretKey,
        string rawData,
        string expectedHash)
    {
        var computed = ComputeHmacSha512(secretKey, rawData);
        return string.Equals(
            computed,
            expectedHash,
            StringComparison.OrdinalIgnoreCase);
    }
}
