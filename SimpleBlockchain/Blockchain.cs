namespace SimpleBlockchain;

public class Blockchain
{
    private readonly List<Transaction> _transactions = new();
    private readonly List<Block> _chain = new();
    
    public Blockchain()
    {
        GenerateGenesisBlock();
    }

    public Block LastBlock => _chain[^1];
    public int CurrentTransactionsCount => _transactions.Count;

    public int NewTransaction(string sender, string recipient, int amount)
    {
        var newTransaction = new Transaction(sender, recipient, amount);
        _transactions.Add(newTransaction);
        return LastBlock.Index + 1;
    }

    private void GenerateGenesisBlock()
    {
        NewBlock(1, "previousHash");
    }

    private Block NewBlock(int proof, string previousHash)
    {
        var newBlock = new Block(_chain.Count + 1, DateTime.Now, new List<string>(), proof, previousHash);
        _chain.Add(newBlock);
        return newBlock;
    }
}