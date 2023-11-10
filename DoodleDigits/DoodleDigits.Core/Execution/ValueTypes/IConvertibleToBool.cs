using DoodleDigits.Core.Parsing.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core.Execution.ValueTypes; 
public interface IConvertibleToBool {
    public BooleanValue ConvertToBool(ExecutionContext context, Expression node);
}
