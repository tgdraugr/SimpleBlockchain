namespace SimpleBlockchain;

public interface IBrewNonce
{
    int NewNonce(Block lastMinedBlock);
}