namespace DoodleDigits.Core.Parsing.Ast.AmbiguousNodes;
public class FunctionDeclarationOrEquals : AmbiguousNode {
    public FunctionDeclaration FunctionDeclaration { get; }
    public Comparison EqualsComparison { get; }

    public FunctionDeclarationOrEquals(FunctionDeclaration function, Comparison comparison, Range range) : base(range) {
        FunctionDeclaration = function;
        EqualsComparison = comparison;
    }

    public FunctionDeclarationOrEquals(FunctionDeclaration function) :base(0..0) {
        FunctionDeclaration = function;

        var identifier = new Identifier(function.Identifier);
        var calls = function.ArgumentNames.Select(x => new Identifier(x)).ToArray();

        EqualsComparison = new Comparison(
            new FunctionCallOrMultiplication(
                new FunctionCall(function.Identifier, calls), 
                new BinaryOperation(
                    identifier,
                    BinaryOperation.OperationType.Multiply,
                    function.ArgumentNames.Length == 1 ? 
                        new Identifier(function.ArgumentNames[0]) : 
                        new VectorDeclaration(calls, 0..0)
                )  
            ),
            Comparison.ComparisonType.Equals,
            function.Implementation
        );
    }

    public override bool Equals(AstNode other) {
        if (other is not FunctionDeclarationOrEquals otherAmbiguous) {
            return false;
        }
        return FunctionDeclaration.Equals(otherAmbiguous.FunctionDeclaration) && EqualsComparison.Equals(otherAmbiguous.EqualsComparison);
    }

    public override string ToString() {
        return FunctionDeclaration.ToString();
    }
}
