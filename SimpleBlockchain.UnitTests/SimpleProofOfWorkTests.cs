using System;
using System.Collections.Generic;
using Xunit;

namespace SimpleBlockchain.UnitTests;

public class SimpleProofOfWorkTests
{
    private static readonly Block DummyBlock = new(
        1, 
        new DateTime(1990, 01, 16, 00, 15, 00), 
        new List<Transaction>(), 
        1, 
        "PreviousHash");
    
    [Fact]
    public void Should_brew_a_nonce_when_hash_has_predefined_count_of_leading_zeros()
    {
        var expectedHashesPerNonce = new Dictionary<int, string>
        {
            { 0, "abc" },
            { 1, "x91" },
            { 2, "ax1" },
            { 3, "0x1" },
            { 4, "00a" },
            { 5, "0a2" }
        };

        var hashProducer = new AnotherHashProducer(expectedHashesPerNonce);
        var pow = new SimpleProofOfWork(hashProducer);
        
        var nonce = pow.NewNonce(DummyBlock);
        
        Assert.Equal(4, nonce);
    }
}

public class AnotherHashProducer : IProduceHash
{
    private int _index;
    private readonly Dictionary<int, string> _hashesPerNonce;

    public AnotherHashProducer(Dictionary<int,string> hashesPerNonce)
    {
        _hashesPerNonce = hashesPerNonce;
    }

    public string GeneratedHash(string input)
    {
        return _hashesPerNonce[_index++];
    }
}

public class SimpleProofOfWork : IBrewNonce
{
    private readonly AnotherHashProducer _hashProducer;

    public SimpleProofOfWork(AnotherHashProducer hashProducer)
    {
        _hashProducer = hashProducer;
    }

    public int NewNonce(Block lastMinedBlock)
    {
        var nonce = 0;
        while (IsValidProof(lastMinedBlock, nonce) is false)
            nonce++;
        
        return nonce;
    }

    private bool IsValidProof(Block lastMinedBlock, int nonce)
    {
        var hash = _hashProducer.GeneratedHash("");
        return hash.StartsWith("00");
    }
}