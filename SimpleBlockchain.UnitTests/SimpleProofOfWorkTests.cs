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

        Assert.Equal(1, hashProducer.PreviousNonceInvoked.Count);
        Assert.Equal(1, hashProducer.PreviousHashInvoked.Count);
        Assert.Contains("1", hashProducer.PreviousNonceInvoked);
        Assert.Contains("PreviousHash", hashProducer.PreviousHashInvoked);
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

    public ISet<string> PreviousNonceInvoked { get; } = new HashSet<string>();
    public ISet<string> PreviousHashInvoked { get; } = new HashSet<string>();

    public string GeneratedHash(string input)
    {
        TrackInvokes(input);
        return _hashesPerNonce[_index++];
    }

    private void TrackInvokes(string input)
    {
        var split = input.Split(":");
        if (split.Length == 0) return;
        PreviousNonceInvoked.Add(split[0]);
        PreviousHashInvoked.Add(split[1]);
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
        var guess = $"{lastMinedBlock.Nonce}:{lastMinedBlock.PreviousHash}:{nonce}";
        var hash = _hashProducer.GeneratedHash(guess);
        return hash.StartsWith("00");
    }
}