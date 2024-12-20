using System;
using scriptableObjects.map;

namespace influence
{
    public class InfluenceGrids
    {
        private double[] _values;
        private double[] _store;
        private double[] _happiness;

        private int _width;
        private int _height;
        private int _depth;

        private int _area;

        public InfluenceGrids(int width, int height)
        {
            _width = width;
            _height = height;
            _depth = Enum.GetValues(typeof(Layer)).Length;

            _area = _width * height;

            _values = new double[_width * _height * _depth];
            _store = new double[_width * _height * _depth];
            _happiness = new double[_width * _height];
        }

        public void Dispose()
        {
        }

        private int GetIndex(Layer layer, int xy)
        {
            return (int)layer * _area + xy;
        }

        private int GetIndex(Layer layer, int x, int y)
        {
            return (int)layer * _area + y * _width + x;
        }

        private int GetIndex(Layer layer)
        {
            return (int)layer * _area;
        }

        public void AddValue(Layer layer, int xy, double amount)
        {
            _values[GetIndex(layer, xy)] += amount;
        }

        public void AddValue(Layer layer, int x, int y, double amount)
        {
            _values[GetIndex(layer, x, y)] += amount;
        }

        public double RemoveValue(Layer layer, int x, int y, double amount)
        {
            int index = GetIndex(layer, x, y);
            double reduction = Math.Min(_values[index], amount);
            _values[index] -= reduction;
            return reduction;
        }

        public double GetValue(Layer layer, int x, int y)
        {
            return _values[GetIndex(layer, x, y)];
        }

        public double GetHappiness(int x, int y)
        {
            return _happiness[y * _width + x];
        }

        public double[] GetValues(Layer layer)
        {
            var result = new double[_area];
            Array.Copy(_values, GetIndex(layer), result, 0, _area);
            return result;
        }

        public double[] GetValues()
        {
            return _values;
        }

        public void SetValues(double[] values)
        {
            _values = values;
        }

        public double[] GetStore()
        {
            return _store;
        }

        public void SetStore(double[] store)
        {
            _store = store;
        }

        public double[] GetHappiness()
        {
            return _happiness;
        }

        public void SetHappiness(double[] happiness)
        {
            _happiness = happiness;
        }
    }
}