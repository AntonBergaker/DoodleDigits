using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits.Core; 
public class ParseError {
    public readonly Range Position;
    public readonly string Message;

    public ParseError(Range position, string message) {
        this.Position = position;
        this.Message = message;
    }

}
