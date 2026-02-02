using System.Net;

namespace Infrastructure.ExternalServices.Payment.Vnpay;

public class VnpaySignatureHelper
{
    public static string BuildRawData(
       IDictionary<string, string> parameters)
    {
        var sortedParams = parameters
        .Where(p => p.Key.StartsWith("vnp_"))
        .Where(p => p.Key != "vnp_SecureHash")
        .OrderBy(p => p.Key, StringComparer.Ordinal);

        return string.Join("&",
            sortedParams.Select(p => $"{p.Key}={WebUtility.UrlEncode(p.Value ?? "")}"));
    }
    public static bool VerifySignature(
        IDictionary<string, string> parameters,
        string hashSecret)
    {
        var rawData = BuildRawData(parameters);
        var secureHash = parameters["vnp_SecureHash"];

        return HmacHelper.VerifyHmacSha512(
            hashSecret,
            rawData,
            secureHash);
    }
}
