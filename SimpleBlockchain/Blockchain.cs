namespace SimpleBlockchain;

public class Blockchain
{
    private readonly List<Block> _chain = new();
    
    public Blockchain()
    {
        GenerateGenesisBlock();
    }

    public Block LastBlock => _chain[^1];

    private void GenerateGenesisBlock()
    {
        NewBlock(1, "previousHash");
    }

    private Block NewBlock(int proof, string previousHash)
    {
        var currentIndex = _chain.Count + 1;
        var newBlock = new Block(currentIndex, DateTime.Now, new List<string>(), proof, previousHash);
        _chain.Add(newBlock);

        return newBlock;
    }
}