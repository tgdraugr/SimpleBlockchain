namespace SimpleBlockchain;

public record Block(int Index, DateTime Timestamp, IList<string> Transactions, int Proof, string PreviousHash);