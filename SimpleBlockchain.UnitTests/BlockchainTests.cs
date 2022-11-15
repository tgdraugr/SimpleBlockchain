using NSubstitute;
using Xunit;

namespace SimpleBlockchain.UnitTests;

public class BlockchainTests
{
    private static readonly FakeNonceBrewer FakeNonceBrewer = new();
    private readonly Blockchain _blockchain = new(new FakeHashProducer(), FakeNonceBrewer, new Neighbors());

    [Fact]
    public void Has_a_genesis_block()
    {
        const int genesisBlockIndex = 1;
        Assert.NotNull(_blockchain.LastMinedBlock);
        Assert.Equal(genesisBlockIndex, _blockchain.LastMinedBlock.Index);
    }

    [Fact]
    public void Adds_new_transaction_into_next_mined_block()
    {
        const int expectedNextMinedBlockIndex = 2;

        for (var count = 1; count <= 3; count++)
        {
            var transaction = _blockchain.NewTransaction("sender", "recipient", count);
            Assert.Equal(expectedNextMinedBlockIndex, transaction.BlockIndex);
            Assert.Equal(count, _blockchain.CurrentTransactionsCount);
        }
    }

    [Fact]
    public void Resets_current_transactions_when_mining_a_new_block()
    {
        const int expectedNumberOfTransactions = 3;
        BroadcastTransactions(expectedNumberOfTransactions);

        var lastMinedBlock = _blockchain.LastMinedBlock; // persist current lastMinedBlock for expectation
        _blockchain.NewMinedBlock();
        
        Assert.Equal(expectedNumberOfTransactions, _blockchain.LastMinedBlock.Transactions.Count);
        Assert.Equal(0, _blockchain.CurrentTransactionsCount);
        Assert.Equal(lastMinedBlock.ToString(), _blockchain.LastMinedBlock.PreviousHash);
    }

    [Fact]
    public void Produces_a_new_nonce_based_on_last_block_when_mining_a_new_one()
    {
        const int expectedNumberOfTransactions = 3;
        BroadcastTransactions(expectedNumberOfTransactions);
        
        var lastMinedBlock = _blockchain.LastMinedBlock; // persist current lastMinedBlock for expectation
        _blockchain.NewMinedBlock();
        
        Assert.True(FakeNonceBrewer.Called);
        Assert.Equal(lastMinedBlock, FakeNonceBrewer.LastMinedBlockReceived);
    }

    [Fact]
    public void Keeps_track_of_neighbor_nodes()
    {
        _blockchain.Register("neighbor1", "neighbor2");
        Assert.Contains("neighbor1", _blockchain.Neighbors);
        Assert.Contains("neighbor2", _blockchain.Neighbors);
    }

    private void BroadcastTransactions(int totalTransactions)
    {
        for (var count = 1; count <= totalTransactions; count++)
            _blockchain.NewTransaction("sender", "recipient", count);
    }
}