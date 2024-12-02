﻿using System;
using scriptableObjects.map;

namespace influence
{
    public class InfluenceGrids
    {
        private double[] _values;

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

        public void RemoveValue(Layer layer, int x, int y, double amount)
        {
            int index = GetIndex(layer, x, y);
            _values[index] = Math.Max(_values[index] - amount, 0);
        }

        public double GetValue(Layer layer, int x, int y)
        {
            return _values[GetIndex(layer, x, y)];
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
    }
}