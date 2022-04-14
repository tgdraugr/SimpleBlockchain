using Xunit;

namespace SimpleBlockchain.UnitTests
{
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
    }
}