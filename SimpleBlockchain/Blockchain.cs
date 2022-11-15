namespace SimpleBlockchain;

public class Blockchain
{
    private readonly List<Transaction> _transactions = new();
    private readonly List<Block> _chain = new();
    
    private readonly IProduceHash _hashProducer;
    private readonly IBrewNonce _nonceBrewer;
    private readonly Neighbors _neighbors;

    public Blockchain(IProduceHash hashProducer, IBrewNonce nonceBrewer)
    {
        _hashProducer = hashProducer;
        _nonceBrewer = nonceBrewer;
        _neighbors = new Neighbors();
        SeedWithGenesisBlock();
    }

    public Block LastMinedBlock => _chain[^1];
    
    public int CurrentTransactionsCount => _transactions.Count;
    
    public IEnumerable<Block> FullChain => _chain.ToArray();

    public IReadOnlyList<string> Neighbors => _neighbors.ToList();

    public Transaction NewTransaction(string sender, string recipient, int amount)
    {
        var newTransaction = new Transaction(sender, recipient, amount, LastMinedBlock.Index + 1);
        _transactions.Add(newTransaction);
        return newTransaction;
    }

    public Block NewMinedBlock()
    {
        var nonce = _nonceBrewer.NewNonce(LastMinedBlock);
        return NewBlock(nonce);
    }

    private Block NewBlock(int nonce, string? previousHash = default)
    {
        var newBlock = new Block(
            _chain.Count + 1, 
            DateTime.Now, 
            _transactions.ToList(), 
            nonce,
            previousHash ?? _hashProducer.GeneratedHash(LastMinedBlock.ToString()));
        
        _chain.Add(newBlock);
        _transactions.Clear();
        return newBlock;
    }
    
    public void Register(params string[] nodes)
    {
        foreach (var node in nodes)
            _neighbors.Add(node);
    }

    private void SeedWithGenesisBlock()
    {
        NewBlock(1, "None");
    }
}