namespace SimpleBlockchain.UnitTests;

internal class FakeHashProducer : IProduceHash
{
    public string GeneratedHash(string input)
    {
        return input;
    }
}