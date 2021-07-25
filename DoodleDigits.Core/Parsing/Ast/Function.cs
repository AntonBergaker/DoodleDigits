﻿using System;
using System.Collections.Generic;
using System.Linq;
using DoodleDigits.Core.Utilities;

namespace DoodleDigits.Core.Parsing.Ast {
    public class Function : Expression {
        public string Identifier { get; }
        public Expression[] Arguments { get; }

        public override Range FullPosition {
            get {
                Range fullPosition = Position;
                foreach (Expression argument in Arguments) {
                    fullPosition = Utils.Join(fullPosition, argument.FullPosition);
                }

                return fullPosition;
            }
        }

        public Function(string identifier, IEnumerable<Expression> arguments, Range position) : base(position) {
            Identifier = identifier;
            Arguments = arguments.ToArray();
        }

        public Function(string identifier, IEnumerable<Expression> arguments) : this(identifier, arguments, 0..0) { }


        public override bool Equals(AstNode other) {
            if (other is not Function function) {
                return false;
            }

            if (function.Arguments.Length != Arguments.Length) {
                return false;
            }

            for (int i = 0; i < Arguments.Length; i++) {
                if (Arguments[i].Equals(function.Arguments[i]) == false) {
                    return false;
                }
            }

            return true;
        }

        public override string ToString() {
            return $"{Identifier}({string.Join(", ", Arguments.Select(x => x.ToString()))})";
        }
    }
}