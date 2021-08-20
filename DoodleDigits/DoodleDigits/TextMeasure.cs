using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DoodleDigits {
    /// <summary>
    /// Measures text before and after calculation, to give results an approximately correct position if the textbox has changed a lot during the calculation
    /// </summary>

    public class TextMeasure {
        private readonly TextBox textBox;

        private record CharacterInfo(char Char, int Line);

        public int Length { get; }

        private readonly CharacterInfo[] characterInfos;

        // The final character on each line of the textbox
        private readonly List<int> lineRanges;

        public TextMeasure(string input, TextBox textBox) {
            lineRanges = new List<int>();
            this.textBox = textBox;
            Length = input.Length;
            characterInfos = new CharacterInfo[Length];

            int line = 0;

            for (int i = 0; i < Length; i++) {
                char c = input[i];

                characterInfos[i] = new CharacterInfo(c, line);
                
                if (c == '\n') {
                    line++;
                }
            }
            
        }

        public void ApplyNewTextBoxDimensions() {
            // Because of WPF weirdness, GetCharacterIndexFromLineIndex is busted with custom margins.
            // Instead of relying on the box itself, before we go to place our labels we figure out what character belongs to once more
            // Hence the need for this function
            
            lineRanges.Clear();

            string text = textBox.Text;
            int lastNonNewline = 0;
            for (int i = 0; i < text.Length; i++) {
                if (text[i] is '\r' or '\n' == false) {
                    lastNonNewline = i;
                }
                if (text[i] == '\n') {
                    lineRanges.Add(lastNonNewline);
                    lastNonNewline = i + 1;
                }
            }

            lineRanges.Add(text.Length-1);
        }

        public int GetLineForIndex(int index) {
            return characterInfos[Math.Min(Length - 1, index)].Line;
        }


        public Rect GetFinalRectOfLine(int line) {
            int index;
            // If linecount is less than line, grab the final index
            if (line >= lineRanges.Count) {
                index = lineRanges[^1];
            } else {
                // To get the final character, get the first index of the following line and go backwards
                index = lineRanges[line];
            }

            return textBox.GetRectFromCharacterIndex(index, true);
        }
    }
}
