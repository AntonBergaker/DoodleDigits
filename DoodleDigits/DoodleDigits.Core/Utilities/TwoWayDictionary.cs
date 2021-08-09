using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DoodleDigits.Core.Utilities {
    public class TwoWayDictionary<T0, T1> : IEnumerable<KeyValuePair<T0, T1>> where T0: notnull where T1 : notnull {
        private readonly Dictionary<T0, T1> dictionary01;
        private readonly Dictionary<T1, T0> dictionary10;

        public T1 this[T0 key] {
            get => dictionary01[key];
            set => dictionary01[key] = value;
        }

        public T0 this[T1 key] {
            get => dictionary10[key];
            set => dictionary10[key] = value;
        }


        public TwoWayDictionary() {
            dictionary01 = new Dictionary<T0, T1>();
            dictionary10 = new Dictionary<T1, T0>();
        }

        public void Add(T0 value0, T1 value1) {
            dictionary01.Add(value0, value1);
            dictionary10.Add(value1, value0);
        }

        public void Remove(T0 value) {
            // Get the key for value1
            if (dictionary01.TryGetValue(value, out T1? otherValue)) {
                dictionary01.Remove(value);
                dictionary10.Remove(otherValue);
            }
        }

        public void Remove(T1 value) {
            // Get the key for value0
            if (dictionary10.TryGetValue(value, out T0? otherValue)) {
                dictionary10.Remove(value);
                dictionary01.Remove(otherValue);
            }
        }

        public bool TryGetValue(T0 key, [NotNullWhen(true)] out T1? value) {
            return dictionary01.TryGetValue(key, out value);
        }

        public bool TryGetValue(T1 key,[NotNullWhen(true)] out T0? value) {
            return dictionary10.TryGetValue(key, out value);
        }
        
        public IEnumerable<T0> Get0Values() {
            return dictionary01.Keys;
        }

        public IEnumerable<T1> Get1Values() {
            return dictionary10.Keys;
        }

        public IEnumerator<KeyValuePair<T0, T1>> GetEnumerator() {
            return dictionary01.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
