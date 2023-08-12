namespace Paraclete.IO;

using Paraclete.Modules.Calculator;

public class CalculatorExpressionInputDefinition : IInputDefinition
{
    public Type DataType => typeof(Expression);
    public string Alphabet => IInputDefinition.NumericAlphabet + "()+-*/%^ ";

    public bool TryCompleteInput(string inputData, out OutResult<object> result)
    {
        if (Expression.TryCreate(inputData, out var expression))
        {
            result = OutResult<object>.CreateSuccessful(expression);
            return true;
        }

        result = OutResult<object>.CreateFailed("Invalid expression");
        return false;
    }
}
