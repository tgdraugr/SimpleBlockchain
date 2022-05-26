using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

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
        return NewBlock(10);
    }

    private Block NewBlock(int proof, string? previousHash = default)
    {
        var newBlock = new Block(
            _chain.Count + 1, 
            DateTime.Now, 
            _transactions.ToList(), 
            proof,
            previousHash ?? GenerateSha256Hash(LastMinedBlock));
        
        _chain.Add(newBlock);
        _transactions.Clear();
        return newBlock;
    }

    private void SeedWithGenesisBlock()
    {
        NewBlock(1, "None");
    }
    
    private static string GenerateSha256Hash(Block block)
    {
        var rawData = JsonConvert.SerializeObject(block);
        return GenerateSha256Hash(rawData);
    }

    private static string GenerateSha256Hash(string rawData)
    {
        using var sha256 = SHA256.Create();
        var hashInBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        var builder = new StringBuilder();
        foreach (var hashByte in hashInBytes)
        {
            builder.Append(hashByte.ToString("x2"));
        }

        return builder.ToString();
    }
}