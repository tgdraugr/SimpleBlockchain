namespace SimpleBlockchain.UnitTests;

public class FakeNonceBrewer : IBrewNonce
{
    public bool CalledProofOfWork { get; private set; }

    public int NewNonce()
    {
        CalledProofOfWork = true;
        return 10;
    }
}