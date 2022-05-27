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
        const string expectedHash = "fd6070a9e77514c58e228308b64ebb118e7d20db3e28029e5140f7f8ee0f9dd6";
        Assert.Equal(expectedHash, sha256Producer.GeneratedHash(DummyBlock));
    }
}