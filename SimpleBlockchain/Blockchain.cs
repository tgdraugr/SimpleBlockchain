namespace SimpleBlockchain;

public class Blockchain
{
    private readonly List<Transaction> _transactions = new();
    private readonly List<Block> _chain = new();
    
    public Blockchain()
    {
        SeedWithGenesisBlock();
    }

    public Block LastMinedBlock => _chain[^1];
    
    public int CurrentTransactionsCount => _transactions.Count;

    public Transaction NewTransaction(string sender, string recipient, int amount)
    {
        var newTransaction = new Transaction(sender, recipient, amount, LastMinedBlock.Index + 1);
        _transactions.Add(newTransaction);
        return newTransaction;
    }

    public Block MineBlock()
    {
        return NewBlock(10, "StaticHash");
    }

    private Block NewBlock(int proof, string previousHash)
    {
        var newBlock = new Block(_chain.Count + 1, DateTime.Now, new List<Transaction>(_transactions), proof, previousHash);
        _chain.Add(newBlock);
        _transactions.Clear();
        return newBlock;
    }

    private void SeedWithGenesisBlock()
    {
        NewBlock(1, "None");
    }
}