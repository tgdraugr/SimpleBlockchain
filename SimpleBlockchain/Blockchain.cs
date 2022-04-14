namespace SimpleBlockchain;

public class Blockchain
{
    private readonly List<Block> _chain = new();
    
    public Blockchain()
    {
        GenerateGenesisBlock();
    }

    public Block LastBlock => _chain.Last();

    private void GenerateGenesisBlock()
    {
        RegisterNewBlock(1, "previous");
    }

    private void RegisterNewBlock(int proof, string previousHash)
    {
        _chain.Add(new Block(1, DateTime.Now, new List<string>(), proof, previousHash));
    }
}