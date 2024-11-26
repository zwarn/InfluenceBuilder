using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace influence
{
    [BurstCompile]
    public class InfluenceGrid
    {
        public readonly int Width;
        public readonly int Height;
        private NativeArray<double> _values;

        public InfluenceGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _values = new NativeArray<double>(Width * Height, Allocator.Persistent);
        }

        public double GetValue(int x, int y)
        {
            return _values[y * Width + x];
        }

        public double[] GetValues()
        {
            return _values.ToArray();
        }

        public NativeArray<double> GetValuesArray()
        {
            return _values;
        }

        public void SetValues(double[] data)
        {
            NativeArray<double> newValues = new NativeArray<double>(data, Allocator.Persistent);
            _values.Dispose();
            _values = newValues;
        }

        public void AddValue(int x, int y, double value)
        {
            _values[y * Width + x] += value;
        }

        public void AddValue(int index, double value)
        {
            _values[index] += value;
        }

        public void RemoveValue(int index, double value)
        {
            _values[index] = math.max(_values[index] - value, 0);
        }

        public void RemoveValue(int x, int y, double value)
        {
            RemoveValue(y * Width + x, value);
        }

        public void Dispose()
        {
            _values.Dispose();
        }

    }
}