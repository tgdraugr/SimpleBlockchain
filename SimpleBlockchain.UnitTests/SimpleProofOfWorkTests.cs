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
    public void Should_brew_a_valid_nonce_for_mining_block()
    {
        var pow = new SimpleProofOfWork();
        var nonce = pow.NewNonce(DummyBlock);
        Assert.Equal(1, nonce);
    }
}

public class SimpleProofOfWork : IBrewNonce
{
    private bool proofState = true;
    
    public int NewNonce(Block lastMinedBlock)
    {
        var nonce = 0;
        while (IsValidProof(lastMinedBlock, nonce) is false)
            nonce++;
        
        return nonce;
    }

    private bool IsValidProof(Block lastMinedBlock, int nonce)
    {
        return proofState = !proofState;
    }
}