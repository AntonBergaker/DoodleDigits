using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DoodleDigits {
    public class LineMeasure {
        private readonly TextBox textBox;

        public string[] Lines;

        private Point[] endPositions;

        public LineMeasure(string input, TextBox textBox) {
            this.textBox = textBox;
            Lines = input.Split("\r\n");

            endPositions = new Point[Lines.Length];
            int charIndex = 0;
            for (int i = 0; i < endPositions.Length; i++) {
                string line = Lines[i];
                endPositions[i] = MeasureLine(line, charIndex);
                charIndex += line.Length+2;
            }
        }

        private Point MeasureLine(string line, int lineStartIndex) {
            if (line.Length == 0) {
                return new Point(-1, -1);
            }

            var zeroChar = textBox.GetRectFromCharacterIndex(lineStartIndex);


            double maxX = zeroChar.Right;
            double maxY = zeroChar.Bottom;
            double minY = zeroChar.Top;

            for (int i = 1; i < line.Length; i++) {
                var rect = textBox.GetRectFromCharacterIndex(lineStartIndex + i);
                minY = Math.Min(minY, rect.Top);
                maxY = Math.Max(maxY, rect.Bottom);
                maxX = Math.Max(maxX, rect.Right);
            }

            return new Point(maxX, (minY + maxY) / 2);
        }

        public Point GetLineEndPosition(int lineIndex) {
            return endPositions[lineIndex];
        }

        public int GetLineIndex(int charIndex) {
            return textBox.GetLineIndexFromCharacterIndex(charIndex);
        }
    }
}
