using Xunit;

namespace SimpleBlockchain.UnitTests;

public class BlockchainTests
{
    private readonly Blockchain _blockchain = new(new FakeHashProducer());

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
            var transaction = _blockchain.NewTransaction("sender", "recipient", 10);
            Assert.Equal(expectedNextMinedBlockIndex, transaction.BlockIndex);
            Assert.Equal(transactionCount, _blockchain.CurrentTransactionsCount);
        }
    }

    [Fact]
    public void Should_reset_current_transactions_on_mining_a_new_block()
    {
        const int expectedNumberOfTransactions = 3;
        for (var transactionCount = 1; transactionCount <= expectedNumberOfTransactions; transactionCount++)
            _blockchain.NewTransaction("sender", "recipient", 10);

        _blockchain.MineBlock();
        Assert.Equal(expectedNumberOfTransactions, _blockchain.LastMinedBlock.Transactions.Count);
        Assert.Equal(0, _blockchain.CurrentTransactionsCount);
        Assert.Equal("0x1", _blockchain.LastMinedBlock.PreviousHash);
    }
}