namespace SimpleBlockchain;

public record Block(int Index, DateTime Timestamp, IList<Transaction> Transactions, int Proof, string PreviousHash);