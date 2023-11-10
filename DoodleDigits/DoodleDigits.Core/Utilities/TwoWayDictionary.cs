using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DoodleDigits.Core.Utilities; 
public class TwoWayDictionary<T0, T1> : IEnumerable<KeyValuePair<T0, T1>> where T0: notnull where T1 : notnull {
    private readonly Dictionary<T0, T1> _dictionary01;
    private readonly Dictionary<T1, T0> _dictionary10;

    public T1 this[T0 key] {
        get => _dictionary01[key];
        set => _dictionary01[key] = value;
    }

    public T0 this[T1 key] {
        get => _dictionary10[key];
        set => _dictionary10[key] = value;
    }


    public TwoWayDictionary() {
        _dictionary01 = new Dictionary<T0, T1>();
        _dictionary10 = new Dictionary<T1, T0>();
    }

    public void Add(T0 value0, T1 value1) {
        _dictionary01.Add(value0, value1);
        _dictionary10.Add(value1, value0);
    }

    public void Remove(T0 value) {
        // Get the key for value1
        if (_dictionary01.TryGetValue(value, out T1? otherValue)) {
            _dictionary01.Remove(value);
            _dictionary10.Remove(otherValue);
        }
    }

    public void Remove(T1 value) {
        // Get the key for value0
        if (_dictionary10.TryGetValue(value, out T0? otherValue)) {
            _dictionary10.Remove(value);
            _dictionary01.Remove(otherValue);
        }
    }

    public bool TryGetValue(T0 key, [NotNullWhen(true)] out T1? value) {
        return _dictionary01.TryGetValue(key, out value);
    }

    public bool TryGetValue(T1 key,[NotNullWhen(true)] out T0? value) {
        return _dictionary10.TryGetValue(key, out value);
    }
    
    public IEnumerable<T0> Get0Values() {
        return _dictionary01.Keys;
    }

    public IEnumerable<T1> Get1Values() {
        return _dictionary10.Keys;
    }

    public IEnumerator<KeyValuePair<T0, T1>> GetEnumerator() {
        return _dictionary01.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
