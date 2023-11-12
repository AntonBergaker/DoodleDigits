using DoodleDigits.Core.Parsing.Ast;

namespace DoodleDigits.Core.Execution.ValueTypes;
public interface IConvertibleToBool {
    public BooleanValue ConvertToBool(ExecutorContext context, Expression node);
}
