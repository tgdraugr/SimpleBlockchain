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
    
    [Fact]
    public async Task Should_add_new_transaction_to_the_blockchain()
    {
        var client = new BlockchainNode.BlockchainNodeClient(Channel);
        
        var reply = await client.BroadcastAsync(new TransactionRequest
        {
            Sender = "A Person",
            Recipient = "Another Person",
            Amount = 50,
        });
        
        Assert.Equal("A Person", reply.Sender);
        Assert.Equal("Another Person", reply.Recipient);
        Assert.Equal(50, reply.Amount);
        Assert.Equal(2, reply.BlockIndex); // since blockchain only contains the genesis block
    }

    [Fact]
    public async Task Should_mine_a_new_block()
    {
        var client = new BlockchainNode.BlockchainNodeClient(Channel);

        var transactionReply = await client.BroadcastAsync(new TransactionRequest
        {
            Sender = "Mining test",
            Recipient = "Fake recipient",
            Amount = 10
        });

        var minedBlock = await client.MineAsync(new Empty());
        
        Assert.Equal(transactionReply.BlockIndex, minedBlock.Index);
        Assert.Contains(transactionReply, minedBlock.Transactions);
    }
}