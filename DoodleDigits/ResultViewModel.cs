using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits {
    public class ResultViewModel {
        public string Content { get; }

        public Point Position { get; }

        public Thickness Margin => new Thickness(Position.X, Position.Y, 0, 0);
        

        public ResultViewModel(string content, Point position) {
            Content = content;
            Position = position;
        }
    }
}
