namespace Paraclete.Modules.Calculator;

public abstract class TokenNodeBase : ITokenNode
{
    public ITokenNode Parent { get; private set; } = ITokenNode.Empty;

    public virtual double Evaluate()
    {
        return 0;
    }

    public void SetParent(ITokenNode parent)
    {
        if (Parent != ITokenNode.Empty)
        {
            throw new InvalidOperationException("Node already has a parent.");
        }

        Parent = parent;
    }
}
