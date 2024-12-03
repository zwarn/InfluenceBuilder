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

        public void AddOrUpdate(Layer layer, T value, Func<T, T, T> mergeFunction)
        {
            if (_data.TryGetValue(layer, out value))
            {
                _data[layer] = mergeFunction(_data[layer], value);
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

        public static Func<double, double, double> Addition()
        {
            return ((a, b) => a + b);
        }
    }
}