using System;
using System.Collections.Generic;
using Xunit;

namespace SimpleBlockchain.UnitTests;

public class Sha256HashProducerTests
{
    private static readonly Block DummyBlock = new(
        1, 
        new DateTime(1990, 01, 16, 00, 15, 00), 
        new List<Transaction>(), 
        1, 
        "PreviousHash");

    [Fact]
    public void Should_produce_a_valid_hash_for_block()
    {
        var sha256Producer = new Sha256HashProducer();
        const string expectedHash = "edbb4b217d7152520f9a48b93782ee950f80052d049992e5c2e3a82afed5a6e3";
        Assert.Equal(expectedHash, sha256Producer.GeneratedHash(DummyBlock.ToString()));
    }
}