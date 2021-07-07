using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Ast
{
    public class UnaryOperation : Expression {

        public enum OperationType {
            Add,
            Subtract,
            Factorial,
            Not
        }

        public static string GetSymbolForType(OperationType operation) {
            return operation switch {
                OperationType.Add => "+",
                OperationType.Subtract => "-",
                _ => throw new NotImplementedException(),
            };
        }

        public static OperationType GetTypeFromToken(TokenType token) {
            return token switch {
                TokenType.Add => OperationType.Add,
                TokenType.Subtract => OperationType.Subtract,
                TokenType.Exclamation => OperationType.Not,
                _ => throw new Exception("Token has no unary operation")
            };
        }


        public OperationType Operation { get; }
        public Expression Value { get; }

        public UnaryOperation(OperationType operation, Expression value, Range position) : base(position) {
            Operation = operation;
            Value = value;
        }

        public UnaryOperation(OperationType operation, Expression value) : this(operation, value, 0..0) { }

        public override bool Equals(AstNode other) {
            if (other is not UnaryOperation uo) {
                return false;
            }

            return uo.Operation == Operation && Value.Equals(uo.Value);
        }

        public override string ToString() {
            if (Operation == OperationType.Factorial) {
                return $"{Value}!";
            }

            return $"{GetSymbolForType(Operation)}{Value}";
        }
    }
}
