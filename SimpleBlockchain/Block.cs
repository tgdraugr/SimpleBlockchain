namespace SimpleBlockchain;

public record Block(int Index, DateTime Timestamp, IList<string> Transactions, int Proof, string PreviousHash)
{
    public override string ToString()
    {
        return $"{Index} ({Timestamp.ToString("yyyy-MM-dd HH:mm:ss")}) | Proof: {Proof}; PrevHash: {PreviousHash}; TrxCount: {Transactions.Count}";
    }
}