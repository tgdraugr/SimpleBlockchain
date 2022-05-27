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

    private static readonly Dictionary<int, string>? ExpectedHashesPerNonce = new()
    {
        { 0, "abc" },
        { 1, "x91" },
        { 2, "ax1" },
        { 3, "0x1" },
        { 4, "00a" },
        { 5, "0a2" },
        { 6, "000" },
        { 7, "ax1" }
    };

    [Fact]
    public void Should_brew_a_nonce_when_hash_has_default_count_of_leading_zeros()
    {
        var fakeHashProducer = new FakeHashProducer(ExpectedHashesPerNonce);
        var pow = new SimpleProofOfWork(fakeHashProducer);
        
        var nonce = pow.NewNonce(DummyBlock);

        Assert.Equal(1, fakeHashProducer.PreviousNonceInvoked.Count);
        Assert.Equal(1, fakeHashProducer.PreviousHashInvoked.Count);
        Assert.Contains(DummyBlock.Nonce.ToString(), fakeHashProducer.PreviousNonceInvoked);
        Assert.Contains(DummyBlock.PreviousHash, fakeHashProducer.PreviousHashInvoked);
        Assert.Equal(new HashSet<int> { 0, 1, 2, 3, 4 }, fakeHashProducer.NonceInvoked);
        Assert.Equal(4, nonce);
    }
}