using Xunit;

namespace SimpleBlockchain.UnitTests
{
    public class BlockchainTests
    {
        [Fact]
        public void Should_have_a_genesis_block()
        {
            var blockchain = new Blockchain();
            Assert.NotNull(blockchain.LastBlock);
        }
    }
}