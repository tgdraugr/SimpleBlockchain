using System.Collections.Generic;

namespace SimpleBlockchain.UnitTests;

internal class FakeHashProducer : IProduceHash
{
    private int _index;
    private readonly Dictionary<int, string>? _hashesPerNonce;

    public FakeHashProducer(Dictionary<int, string>? hashesPerNonce = default)
    {
        _hashesPerNonce = hashesPerNonce;
    }
    
    public ISet<string> PreviousNonceInvoked { get; } = new HashSet<string>();
    public ISet<string> PreviousHashInvoked { get; } = new HashSet<string>();
    public ISet<int> NonceInvoked { get; } = new HashSet<int>();

    public string GeneratedHash(string input)
    {
        if (_hashesPerNonce == null) return input;
        TrackInvokes(input);
        return _hashesPerNonce?[_index++] ?? string.Empty;
    }

    private void TrackInvokes(string input)
    {
        var splitInput = input.Split(":");
        if (splitInput.Length == 0) return;
        PreviousNonceInvoked.Add(splitInput[0]);
        PreviousHashInvoked.Add(splitInput[1]);
        NonceInvoked.Add(int.Parse(splitInput[2]));
    }
}