using System.Collections.Generic;

namespace SimpleBlockchain.UnitTests;

public class FakeHashProducerWithTracking : IProduceHash
{
    private int _index;
    private readonly Dictionary<int,string> _hashesPerNonce;

    public FakeHashProducerWithTracking(Dictionary<int,string> hashesPerNonce)
    {
        _hashesPerNonce = hashesPerNonce;
    }

    public ISet<string> PreviousNonceInvoked { get; } = new HashSet<string>();
    public ISet<string> PreviousHashInvoked { get; } = new HashSet<string>();
    public ISet<int> NonceInvoked { get; } = new HashSet<int>();

    public string GeneratedHash(string input)
    {
        TrackInvokes(input);
        return _hashesPerNonce[_index++];
    }

    private void TrackInvokes(string input)
    {
        var split = input.Split(":");
        if (split.Length == 0) return;
        PreviousNonceInvoked.Add(split[0]);
        PreviousHashInvoked.Add(split[1]);
        NonceInvoked.Add(int.Parse(split[2]));
    }
}