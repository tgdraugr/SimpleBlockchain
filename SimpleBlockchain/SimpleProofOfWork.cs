﻿namespace SimpleBlockchain;

public class SimpleProofOfWork : IBrewNonce
{
    private const int DefaultLeadingZeros = 2;
    private readonly IProduceHash _hashProducer;
    private readonly int _defaultLeadingZeros;

    public SimpleProofOfWork(IProduceHash hashProducer, int defaultLeadingZeros = DefaultLeadingZeros)
    {
        _hashProducer = hashProducer;
        _defaultLeadingZeros = defaultLeadingZeros;
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
        return hash.StartsWith(new string('0', _defaultLeadingZeros));
    }
}