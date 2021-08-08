using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DoodleDigits {


    public class TextMeasure {
        private record CharacterInfo(char Char, int Line, Rect Position);

        public int Length { get; }

        private readonly CharacterInfo[] characterInfos;

        private readonly Rect[] lineRects;

        public TextMeasure(string input, TextBox textBox) {
            Length = input.Length;
            characterInfos = new CharacterInfo[Length];

            List<Rect> lineRectList = new();

            int line = 0;
            Rect? lineRect = null;
            
            for (int i = 0; i < Length; i++) {
                char c = input[i];
                Rect rect = textBox.GetRectFromCharacterIndex(i);

                if (c is '\n' or '\r' == false) {
                    lineRect = lineRect == null ? rect : Rect.Union(lineRect.Value, rect);
                }

                characterInfos[i] = new CharacterInfo(c, line, rect);
                
                if (c == '\n') {
                    lineRectList.Add(lineRect ?? rect);
                    lineRect = null;
                    line++;
                }
            }

            if (lineRect != null) {
                lineRectList.Add(lineRect.Value);
            }

            lineRects = lineRectList.ToArray();
        }

        public int GetLineForIndex(int index) {
            return characterInfos[Math.Min(Length - 1, index)].Line;
        }

        public Rect GetRectForIndex(int index) {
            return characterInfos[Math.Min(Length - 1, index)].Position;
        }

        public Rect GetRectForLine(int line) {
            return lineRects[line];
        }
    }
}
