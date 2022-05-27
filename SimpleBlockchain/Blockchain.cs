namespace SimpleBlockchain;

public class Blockchain
{
    private readonly List<Transaction> _transactions = new();
    private readonly List<Block> _chain = new();
    private readonly IProduceHash _hashProducer;
    
    public Blockchain() 
        : this(new Sha256HashProducer())
    { }

    public Blockchain(IProduceHash hashProducer)
    {
        _hashProducer = hashProducer;
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

    private Block NewBlock(int nonce, string? previousHash = default)
    {
        var newBlock = new Block(
            _chain.Count + 1, 
            DateTime.Now, 
            _transactions.ToList(), 
            nonce,
            previousHash ?? _hashProducer.GeneratedHash(LastMinedBlock));
        
        _chain.Add(newBlock);
        _transactions.Clear();
        return newBlock;
    }

    private void SeedWithGenesisBlock()
    {
        NewBlock(1, "None");
    }
}