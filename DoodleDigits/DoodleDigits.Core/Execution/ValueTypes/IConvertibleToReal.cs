using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.ValueTypes;
public interface IConvertibleToReal {
    public RealValue ConvertToReal(ExecutionContext context, Expression expression);
}
