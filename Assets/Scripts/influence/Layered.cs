using System.Linq;
using UnityEngine;

namespace influence
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Layered<T>
    {
        private Dictionary<Layer, T> _data = new();

        public Layered()
        {
        }

        public Layered(IEnumerable<KeyValuePair<Layer, T>> initialData, Func<T, T, T> mergeFunction)
        {
            foreach (var kvp in initialData)
            {
                AddOrUpdate(kvp.Key, kvp.Value, mergeFunction);
            }
        }

        private Layered(IEnumerable<KeyValuePair<Layer, T>> initalData)
        {
            _data = initalData.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public void AddOrUpdate(Layer layer, T value, Func<T, T, T> mergeFunction)
        {
            if (_data.TryGetValue(layer, out T found))
            {
                _data[layer] = mergeFunction(found, value);
            }
            else
            {
                _data[layer] = value;
            }
        }

        public T Get(Layer layer)
        {
            return _data.TryGetValue(layer, out var value) ? value : default!;
        }

        public Dictionary<Layer, T> ToDictionary()
        {
            return new Dictionary<Layer, T>(_data);
        }

        public void ForEach(Action<Layer, T> action)
        {
            foreach (var (key, value) in _data)
            {
                action.Invoke(key, value);
            }
        }

        public static Func<double, double, double> Plus()
        {
            return ((a, b) => a + b);
        }
        
        public static Func<double, double, double> MinusMin0()
        {
            return ((a, b) => Math.Max(a - b, 0d));
        }

        public static Func<T, T, T> Override()
        {
            return ((a, b) => a);
        }

        public Layered<S> Select<S>(Func<T, S> func)
        {
            return new Layered<S>(_data.Select(pair => new KeyValuePair<Layer, S>(pair.Key, func.Invoke(pair.Value))));
        }
        
    }
}