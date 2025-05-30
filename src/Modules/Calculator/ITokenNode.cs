namespace Paraclete.Modules.Calculator;

public interface ITokenNode
{
    static readonly ITokenNode Empty = new EmptyTokenNodeImplementation();

    ITokenNode Parent { get; }

    void SetParent(ITokenNode parent);
    double Evaluate();
}
