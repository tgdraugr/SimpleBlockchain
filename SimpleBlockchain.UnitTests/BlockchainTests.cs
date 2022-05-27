using Xunit;

namespace SimpleBlockchain.UnitTests;

public class BlockchainTests
{
    private static readonly FakeNonceBrewer FakeNonceBrewer = new();
    private readonly Blockchain _blockchain = new(new FakeHashProducer(), FakeNonceBrewer);

    [Fact]
    public void Should_have_a_genesis_block()
    {
        const int genesisBlockIndex = 1;
        Assert.NotNull(_blockchain.LastMinedBlock);
        Assert.Equal(genesisBlockIndex, _blockchain.LastMinedBlock.Index);
    }

    [Fact]
    public void Should_add_new_transaction_into_next_mined_block()
    {
        const int expectedNextMinedBlockIndex = 2;

        for (var transactionCount = 1; transactionCount <= 3; transactionCount++)
        {
            var transaction = _blockchain.NewTransaction("sender", "recipient", transactionCount);
            Assert.Equal(expectedNextMinedBlockIndex, transaction.BlockIndex);
            Assert.Equal(transactionCount, _blockchain.CurrentTransactionsCount);
        }
    }

    [Fact]
    public void Should_reset_current_transactions_on_mining_a_new_block()
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
    public void Should_produce_a_new_nonce_based_on_last_block_when_mining_a_new_one()
    {
        const int expectedNumberOfTransactions = 3;
        BroadcastTransactions(expectedNumberOfTransactions);
        
        var lastMinedBlock = _blockchain.LastMinedBlock; // persist current lastMinedBlock for expectation
        _blockchain.NewMinedBlock();
        
        Assert.True(FakeNonceBrewer.Called);
        Assert.Equal(lastMinedBlock, FakeNonceBrewer.LastMinedBlockReceived);
    }

    private void BroadcastTransactions(int totalTransactions)
    {
        for (var transactionCount = 1; transactionCount <= totalTransactions; transactionCount++)
            _blockchain.NewTransaction("sender", "recipient", transactionCount);
    }
}