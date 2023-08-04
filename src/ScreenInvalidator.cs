namespace Paraclete;

public partial class ScreenInvalidator
{
    private PaintScope? _scope;

    public bool AreAllInvalid { get; private set; }
    public HashSet<int> InvalidPaneIndices { get; } = new ();

    public void InvalidateAll()
    {
        if (_scope != null)
        {
            _scope.AreAllInvalid = true;
        }
        else
        {
            AreAllInvalid = true;
        }
    }

    public void InvalidatePane(int index)
    {
        if (_scope != null)
        {
            _scope.InvalidPaneIndices.Add(index);
        }
        else
        {
            InvalidPaneIndices.Add(index);
        }
    }

    public IDisposable BeginPaint()
    {
        if (_scope != null)
        {
            throw new InvalidOperationException("Another paint scope is already in progress.");
        }

        _scope = new PaintScope(this);
        return _scope.BeginPaint();
    }

    private void EndPaint()
    {
        if (_scope == null)
        {
            throw new InvalidOperationException("No paint scope in progress.");
        }

        var scope = _scope;
        _scope = null;

        AreAllInvalid = scope.AreAllInvalid;
        InvalidPaneIndices.Clear();
        foreach (var idx in scope.InvalidPaneIndices)
        {
            InvalidPaneIndices.Add(idx);
        }
    }
}

public partial class ScreenInvalidator
{
    private class PaintScope : IDisposable
    {
        private readonly ScreenInvalidator _invalidator;

        public PaintScope(ScreenInvalidator invalidator)
        {
            _invalidator = invalidator;
        }

        public bool AreAllInvalid { get; set; }
        public HashSet<int> InvalidPaneIndices { get; } = new ();

        public PaintScope BeginPaint()
        {
            return this;
        }

        public void Dispose()
        {
            _invalidator.EndPaint();
        }
    }
}
