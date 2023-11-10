namespace DoodleDigits.Core.Parsing.Ast;
public class BaseCast : Expression {

    public enum TargetType {
        Decimal,
        Binary,
        Hex,
        Unknown
    }

    public static TargetType StringToTarget(string text) {
        return text switch {
            "decimal" => TargetType.Decimal,
            "base10" => TargetType.Decimal,
            "binary" => TargetType.Binary,
            "base2" => TargetType.Binary,
            "hex" => TargetType.Hex,
            "hexadecimal" => TargetType.Hex,
            "base16" => TargetType.Hex,
            _ => TargetType.Unknown,
        };
    }

    public static string TargetToString(TargetType type) {
        return type switch {
            TargetType.Decimal => "decimal",
            TargetType.Hex => "hex",
            TargetType.Binary => "binary",
            _ => "unknown"
        };
    }

    public Expression Expression { get; }
    public TargetType Target { get; }

    public BaseCast(Expression expression, TargetType target, Range position) : base(position) {
        Expression = expression;
        Target = target;
    }

    public BaseCast(Expression expression, TargetType target) : this(expression, target, 0..0) {
    }

    public override bool Equals(AstNode other) {
        if (other is not BaseCast bc) {
            return false;
        }

        return bc.Expression.Equals(Expression) && bc.Target == Target;
    }

    public override string ToString() {
        return $"({Expression} as {TargetToString(Target)})";
    }
}
