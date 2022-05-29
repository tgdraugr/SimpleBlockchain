using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace SimpleBlockchain.API.Services;

public class BlockchainNodeService : BlockchainNode.BlockchainNodeBase
{
    private readonly Blockchain _blockchain;

    public BlockchainNodeService(Blockchain blockchain)
    {
        _blockchain = blockchain;
    }

    public override Task<GetChainReply> GetChain(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new GetChainReply
        {
            Chain = { new RepeatedField<Block>
            {
                _blockchain.FullChain.Select(block => new Block()
                {                
                    Index = block.Index,
                    Nonce = block.Nonce,
                    Timestamp = block.Timestamp.ToString("O"),
                    PreviousHash = block.PreviousHash,
                    Transactions =
                    {
                        new RepeatedField<Transaction>
                        {
                            block.Transactions.Select(transaction => new Transaction
                            {
                                Amount = transaction.Amount,
                                Recipient = transaction.Recipient,
                                Sender = transaction.Sender,
                                BlockIndex = transaction.BlockIndex
                            })
                        }
                    }
                })
            } },
            Length = _blockchain.FullChain.Count()
        });
    }
}