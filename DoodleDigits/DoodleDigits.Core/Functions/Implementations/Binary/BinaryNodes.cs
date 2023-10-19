using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Parsing.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
