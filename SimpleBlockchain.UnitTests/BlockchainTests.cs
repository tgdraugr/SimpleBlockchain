using Xunit;

namespace SimpleBlockchain.UnitTests;

public class BlockchainTests
{
    [Fact]
    public void Should_have_a_genesis_block()
    {
        const int genesisBlockIndex = 1;
        var blockchain = new Blockchain();
        Assert.NotNull(blockchain.LastBlock);
        Assert.Equal(genesisBlockIndex, blockchain.LastBlock.Index);
    }

    [Fact]
    public void Should_add_new_transaction_into_next_mined_block()
    {
        var blockchain = new Blockchain();
        var blockIndex = blockchain.NewTransaction("sender", "recipient", 10);
        Assert.Equal(2, blockIndex);
        Assert.Equal(1, blockchain.CurrentTransactionsCount);
    }
}