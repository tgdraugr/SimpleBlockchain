using System;
using System.Collections.Generic;
using System.Linq;
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
        { 0, "abca" },
        { 1, "x91b" },
        { 2, "ax1c" },
        { 3, "0x1d" },
        { 4, "00ae" },
        { 5, "0a2f" },
        { 6, "000g" },
        { 7, "0000" }
    };

    private readonly FakeHashProducer _fakeHashProducer = new(ExpectedHashesPerNonce);
    
    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 4)]
    [InlineData(3, 6)]
    [InlineData(4, 7)]
    public void Should_brew_nonce_when_hash_has_a_given_count_of_leading_zeros(int leadingZeros, int expectedNonce)
    {
        var pow = new SimpleProofOfWork(_fakeHashProducer, leadingZeros);
        var nonce = pow.NewNonce(DummyBlock);
        VerifyNonceBrewing(expectedNonce, nonce);
    }

    private void VerifyNonceBrewing(int expectedNonce, int nonce)
    {
        Assert.Equal(1, _fakeHashProducer.PreviousNonceInvoked.Count);
        Assert.Equal(1, _fakeHashProducer.PreviousHashInvoked.Count);
        Assert.Contains(DummyBlock.Nonce.ToString(), _fakeHashProducer.PreviousNonceInvoked);
        Assert.Contains(DummyBlock.PreviousHash, _fakeHashProducer.PreviousHashInvoked);
        Assert.Equal(Enumerable.Range(0, expectedNonce + 1), _fakeHashProducer.NonceInvoked);
        Assert.Equal(expectedNonce, nonce);
    }
}