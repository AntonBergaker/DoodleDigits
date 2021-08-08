using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DoodleDigits.Collisions {
    public class CellDictionary<T> {
        private readonly Dictionary<long, T> dictionary;

        public CellDictionary() {
            dictionary = new Dictionary<long, T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long MakeKey(int x, int y) {
            return (long) (uint) x << 32 | (uint) y;
        }

        public void Add(int x, int y, T value) {
            dictionary.Add(MakeKey(x, y), value);
        }

        public bool TryGetValue(int x, int y, [NotNullWhen(true)] out T? value) {
            return dictionary.TryGetValue(MakeKey(x, y), out value!);
        }


        public T this[int x, int y] {
            set => dictionary[MakeKey(x, y)] = value;
            get => dictionary[MakeKey(x, y)];
        }


        public IEnumerable<T> Values => dictionary.Values;
    }

}
