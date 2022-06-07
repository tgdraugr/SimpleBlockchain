using System.Collections;

namespace SimpleBlockchain;

public class Neighbors : IEnumerable<string>
{
    private readonly HashSet<string> _neighbors = new();

    public void Add(string node)
    {
        _neighbors.Add(node);
    }

    public IEnumerator<string> GetEnumerator()
    {
        return _neighbors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}