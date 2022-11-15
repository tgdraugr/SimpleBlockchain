using NSubstitute;
using Xunit;

namespace SimpleBlockchain.UnitTests;

public class BlockchainTests
{
    private static readonly Sha256HashProducer HashProducer = new();
    private static readonly SimpleProofOfWork PoW = new(HashProducer, 0);

    [Fact]
    public void Has_a_genesis_block()
    {
        var blockchain = new Blockchain(HashProducer, PoW);
        Assert.NotNull(blockchain.LastMinedBlock);
        Assert.Equal(1, blockchain.LastMinedBlock.Index);
    }

    [Fact]
    public void Adds_new_transaction_into_next_mined_block()
    {       
        var blockchain = new Blockchain(HashProducer, PoW);
        for (var count = 1; count <= 3; count++)
        {
            var transaction = blockchain.NewTransaction("sender", "recipient", count);
            Assert.Equal(2, transaction.BlockIndex);
            Assert.Equal(count, blockchain.CurrentTransactionsCount);
        }
    }

    [Fact]
    public void Resets_current_transactions_when_mining_a_new_block()
    {
        var blockchain = new Blockchain(HashProducer, PoW);

        for (var count = 1; count <= 3; count++)
            blockchain.NewTransaction("sender", "recipient", count);

        blockchain.NewMinedBlock();
        
        Assert.Equal(3, blockchain.LastMinedBlock.Transactions.Count);
        Assert.Equal(0, blockchain.CurrentTransactionsCount);
    }

    [Fact]
    public void Produces_a_new_nonce_based_on_last_block_when_mining_a_new_one()
    {
        var mockNonceBrewer = Substitute.For<IBrewNonce>();
        var blockchain = new Blockchain(HashProducer, mockNonceBrewer);
        var lastMinedBlock = blockchain.LastMinedBlock; // persist current lastMinedBlock for expectation
        
        for (var count = 1; count <= 3; count++)
            blockchain.NewTransaction("sender", "recipient", count);
        
        blockchain.NewMinedBlock();
        
        mockNonceBrewer.Received().NewNonce(lastMinedBlock);
    }

    [Fact]
    public void Keeps_track_of_neighbor_nodes()
    {        
        var blockchain = new Blockchain(HashProducer, PoW);

        blockchain.Register("neighbor1", "neighbor2");
        
        Assert.Contains("neighbor1", blockchain.Neighbors);
        Assert.Contains("neighbor2", blockchain.Neighbors);
    }
}