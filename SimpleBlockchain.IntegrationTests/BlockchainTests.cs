using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using SimpleBlockchain.API;
using Xunit;

namespace SimpleBlockchain.IntegrationTests;

public class BlockchainTests : IntegrationTest
{
    public BlockchainTests(GrpcTestFixture<Startup> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_get_the_full_blockchain_containing_only_the_genesis_block()
    {
        var client = new BlockchainNode.BlockchainNodeClient(Channel);
        var reply = await client.GetChainAsync(new Empty());
        
        Assert.Equal(1, reply.Length);
        var genesisBlock = reply.Chain[0];
        Assert.NotNull(genesisBlock);
        Assert.Equal(1, genesisBlock.Index);
        Assert.Empty(genesisBlock.Transactions);
        Assert.Equal("None", genesisBlock.PreviousHash);
    }
}