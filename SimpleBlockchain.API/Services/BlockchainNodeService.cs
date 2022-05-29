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
            Chain = { new RepeatedField<BlockReply>
            {
                _blockchain.FullChain.Select(block => new BlockReply
                {                
                    Index = block.Index,
                    Nonce = block.Nonce,
                    Timestamp = block.Timestamp.ToString("O"),
                    PreviousHash = block.PreviousHash,
                    Transactions =
                    {
                        new RepeatedField<TransactionReply>
                        {
                            block.Transactions.Select(transaction => new TransactionReply
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

    public override Task<TransactionReply> Broadcast(TransactionRequest request, ServerCallContext context)
    {
        var transaction = _blockchain.NewTransaction(request.Sender, request.Recipient, request.Amount);
        return Task.FromResult(new TransactionReply
        {
            Amount = transaction.Amount,
            Recipient = transaction.Recipient,
            Sender = transaction.Sender,
            BlockIndex = transaction.BlockIndex
        });
    }
}