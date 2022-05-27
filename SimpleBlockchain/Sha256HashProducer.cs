using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace SimpleBlockchain;

public class Sha256HashProducer : IProduceHash
{
    public string GeneratedHash(Block block)
    {
        var rawData = JsonConvert.SerializeObject(block);
        return GeneratedHash(rawData);
    }

    private static string GeneratedHash(string rawData)
    {
        using var sha256 = SHA256.Create();
        var hashInBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        var builder = new StringBuilder();
        foreach (var hashByte in hashInBytes)
        {
            builder.Append(hashByte.ToString("x2"));
        }

        return builder.ToString();
    }
}