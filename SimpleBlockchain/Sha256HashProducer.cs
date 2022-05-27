using System.Security.Cryptography;
using System.Text;

namespace SimpleBlockchain;

public class Sha256HashProducer : IProduceHash
{
    public string GeneratedHash(string input)
    {
        using var sha256 = SHA256.Create();
        var hashInBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

        var builder = new StringBuilder();
        foreach (var hashByte in hashInBytes)
        {
            builder.Append(hashByte.ToString("x2"));
        }

        return builder.ToString();
    }
}