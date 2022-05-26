namespace SimpleBlockchain;

public interface IProduceHash
{
    string GeneratedHash(Block block);
}