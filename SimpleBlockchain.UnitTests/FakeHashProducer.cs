using System.Collections.Generic;

namespace SimpleBlockchain.UnitTests;

internal class FakeHashProducer : IProduceHash
{
    public FakeHashProducer(Dictionary<int, string>? hashesPerNonce = default)
    { }

    public string GeneratedHash(string input)
    {
        return input;
    }
}