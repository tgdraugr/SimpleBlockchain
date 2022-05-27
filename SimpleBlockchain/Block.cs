namespace SimpleBlockchain;

public record Block(int Index, DateTime Timestamp, IList<Transaction> Transactions, int Nonce, string PreviousHash);