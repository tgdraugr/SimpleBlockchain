using Newtonsoft.Json;

namespace SimpleBlockchain;

public class Blockchain
{
    private readonly List<Transaction> _transactions = new();
    private readonly List<Block> _chain = new();
    private readonly IProduceHash _hashProvider;
    
    public Blockchain() 
        : this(new Sha256HashProducer())
    { }

    public Blockchain(IProduceHash hashProvider)
    {
        _hashProvider = hashProvider;
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
        return NewBlock(10);
    }

    private Block NewBlock(int proof, string? previousHash = default)
    {
        var newBlock = new Block(
            _chain.Count + 1, 
            DateTime.Now, 
            _transactions.ToList(), 
            proof,
            previousHash ?? _hashProvider.GeneratedHash(LastMinedBlock));
        
        _chain.Add(newBlock);
        _transactions.Clear();
        return newBlock;
    }

    private void SeedWithGenesisBlock()
    {
        NewBlock(1, "None");
    }
}