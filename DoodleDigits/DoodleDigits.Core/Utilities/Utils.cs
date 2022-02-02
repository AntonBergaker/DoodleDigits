using System;
using System.Collections.Generic;
using System.Linq;

namespace DoodleDigits.Core.Utilities {
    public static class Utils {
        public static Range Join(Range a, Range b) {
            int a0 = a.Start.GetOffset(int.MaxValue);
            int a1 = a.End.GetOffset(int.MaxValue);
            int b0 = b.Start.GetOffset(int.MaxValue);
            int b1 = b.End.GetOffset(int.MaxValue);

            return new Range(Math.Min(a0, b0), Math.Max(a1, b1));
        }

        public static Range Join(Range a, Range b, Range c) {
            return Join(Join(a, b), c);
        }

        public static Range Join(Range a, Range b, Range c, Range d) {
            return Join(Join(a, b), Join(c, d));
        }

        public static Range Join(params Range[] ranges) {
            Range r = ranges[0];
            for (int i = 1; i < ranges.Length; i++) {
                r = Join(r, ranges[i]);
            }

            return r;
        }

        public static Range Join(IEnumerable<Range> ranges) {
            return Join(ranges.ToArray());
        }

        public static T[,] RemoveRowAndColumnFromArray<T>(T[,] array, int row, int column) {
            T[,] result = new T[array.GetLength(0) - 1, array.GetLength(1) - 1];

            for (int i = 0, j = 0; i < array.GetLength(0); i++) {
                if (i == row)
                    continue;

                for (int k = 0, u = 0; k < array.GetLength(1); k++) {
                    if (k == column)
                        continue;

                    result[j, u] = array[i, k];
                    u++;
                }
                j++;
            }

            return result;
        }
    }
}
