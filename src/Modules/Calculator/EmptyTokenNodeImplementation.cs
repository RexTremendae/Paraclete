namespace Paraclete.Modules.Calculator;

[ExcludeFromEnumeration]
public class EmptyTokenNodeImplementation : ITokenNode
{
    public ITokenNode Parent => ITokenNode.Empty;

    public double Evaluate() => 0;

    public void SetParent(ITokenNode parent) => throw new NotSupportedException();
}
