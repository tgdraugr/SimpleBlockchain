namespace SimpleBlockchain;

public class SimpleProofOfWork : IBrewNonce
{
    private const int DefaultLeadingZerosCount = 2;
    private readonly IProduceHash _hashProducer;
    private readonly int _leadingZerosCount;

    public SimpleProofOfWork(IProduceHash hashProducer, int leadingZerosCount = DefaultLeadingZerosCount)
    {
        _hashProducer = hashProducer;
        _leadingZerosCount = leadingZerosCount;
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
        return hash.StartsWith(new string('0', _leadingZerosCount));
    }
}