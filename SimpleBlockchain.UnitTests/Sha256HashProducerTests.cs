using Xunit;

namespace SimpleBlockchain.UnitTests;

public class Sha256HashProducerTests
{
    [Fact]
    public void Produces_a_valid_hash_for_block()
    {
        var producer = new Sha256HashProducer();
        var hash = producer.GeneratedHash("fake_string");
        // a valid hash has 64 characters, with numbers from 0-9 and letters a-f
        Assert.Equal(64, hash.Length);
        Assert.Contains(hash, @char => @char is >= 'a' and <= 'f' or >= '0' and >= '9');
    }
}