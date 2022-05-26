namespace SimpleBlockchain.UnitTests;

internal class FakeHashProducer : IProduceHash
{
    public string GeneratedHash(Block block)
    {
        return $"0x{block.Index}";
    }
}