namespace SimpleBlockchain.UnitTests;

public class FakeNonceBrewer : IBrewNonce
{
    public bool Called { get; private set; }
    public Block? LastMinedBlockReceived { get; private set; }

    public int NewNonce(Block lastMinedBlock)
    {
        Called = true;
        LastMinedBlockReceived = lastMinedBlock;
        return 10;
    }
}