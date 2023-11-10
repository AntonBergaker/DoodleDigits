using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Functions.Implementations.Binary;
public class BinaryNodes {
    public Expression Operation;
    public Expression Lhs;
    public Expression Rhs;

    public BinaryNodes(BinaryOperation operation) {
        Operation = operation;
        Lhs = operation.Lhs;
        Rhs = operation.Rhs;
    }

    public BinaryNodes(Expression operation, Expression lhs, Expression rhs) {
        Operation = operation;
        Lhs = lhs;
        Rhs = rhs;
    }
}
