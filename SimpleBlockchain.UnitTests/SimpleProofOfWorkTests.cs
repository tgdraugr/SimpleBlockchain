using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
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

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 4)]
    [InlineData(3, 6)]
    [InlineData(4, 7)]
    public void Brews_nonce_when_hash_has_a_given_count_of_leading_zeros(int leadingZeros, int expectedNonce)
    {
        // Arrange
        var stubProduceHash = Substitute.For<IProduceHash>();
        stubProduceHash.GeneratedHash(Arg.Any<string>())
            .Returns(
                ExpectedHashesPerNonce?.Values.FirstOrDefault(), 
                ExpectedHashesPerNonce?.Values.Skip(1).ToArray()
                );

        var pow = new SimpleProofOfWork(stubProduceHash, leadingZeros);
        
        // Act
        var nonce = pow.NewNonce(DummyBlock);
        
        // Assert
        Assert.Equal(expectedNonce, nonce);
    }
}