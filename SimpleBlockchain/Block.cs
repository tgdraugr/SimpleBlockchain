namespace SimpleBlockchain;

public record Block(int Index, DateTime Timestamp, IList<Transaction> Transactions, int Nonce, string PreviousHash)
{
    public override string ToString()
    {
        return $"{Index}:{Timestamp.ToString("O")}:{Nonce}:{PreviousHash}";
    }
}