namespace Paraclete.IO;

using Paraclete.Modules.Calculator;

public class CalculatorExpressionInputDefinition : IInputDefinition
{
    public Type DataType => typeof(Expression);
    public string Alphabet => IInputDefinition.NumericAlphabet + "()+-*/%^ ";

    public bool TryCompleteInput(string inputData, out object result, out string errorMessage)
    {
        errorMessage = string.Empty;
        result = Expression.Empty;
        if (!Expression.TryCreate(inputData, out var expression))
        {
            return false;
        }

        result = expression;
        return true;
    }
}
