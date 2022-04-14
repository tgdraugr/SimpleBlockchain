using Xunit;

namespace SimpleBlockchain.UnitTests;

public class BlockchainTests
{
    private readonly Blockchain _blockchain = new();

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
}