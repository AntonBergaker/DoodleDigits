using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Parsing.Ast.AmbiguousNodes;
public class FunctionCallOrMultiplication : AmbiguousNode {
    public FunctionCallOrMultiplication(FunctionCall function, BinaryOperation multiplication) : base(function.Position) {
        Function = function;
        Multiplication = multiplication;
    }

    public FunctionCall Function { get; }
    public BinaryOperation Multiplication { get; }

    public override bool Equals(AstNode other) {
        if (other is not FunctionCallOrMultiplication typedOther) {
            return false;
        }
        return Function.Equals(typedOther.Function) && Multiplication.Equals(typedOther.Multiplication);
    }

    public override string ToString() {
        return Function.ToString();
    }
}
