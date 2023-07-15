namespace Paraclete.Calculator;

using System.Text;

public interface ITokenNode
{
    public static readonly ITokenNode Empty = new EmptyTokenNodeImplementation();

    ITokenNode Parent { get; }

    void SetParent(ITokenNode parent);
    double Evaluate();

    [ExcludeFromEnumeration]
    public class EmptyTokenNodeImplementation : ITokenNode
    {
        public ITokenNode Parent => ITokenNode.Empty;

        public double Evaluate() => 0;

        public void SetParent(ITokenNode parent) => throw new NotSupportedException();
    }
}

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
