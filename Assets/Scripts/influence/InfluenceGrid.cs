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
        private NativeArray<double> _liquidity;

        public InfluenceGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _values = new NativeArray<double>(Width * Height, Allocator.Persistent);
            _liquidity = new NativeArray<double>(Width * Height, Allocator.Persistent);
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

        public NativeArray<double> GetLiquidityArray()
        {
            return _liquidity;
        }
        
        
        public double[] GetLiquidity()
        {
            return _liquidity.ToArray();
        }

        public void SetValues(double[] data)
        {
            NativeArray<double> newValues = new NativeArray<double>(data, Allocator.Persistent);
            _values.Dispose();
            _values = newValues;
        }

        public void SetLiquidity(double[] data)
        {
            NativeArray<double> mewLiquidity = new NativeArray<double>(data, Allocator.Persistent);
            _liquidity.Dispose();
            _liquidity = mewLiquidity;
        }

        public void SetValue(int x, int y, double value)
        {
            _values[y * Width + x] = value;
        }

        public void SetLiquidity(int x, int y, double value)
        {
            SetLiquidity(y * Width + x, value);
        }

        public void SetLiquidity(int index, double value)
        {
            _liquidity[index] = value;
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

        public void RemoveValues(double[] loss)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = math.max(_values[i] - loss[i], 0);
            }
        }

        public void Dispose()
        {
            _values.Dispose();
            _liquidity.Dispose();
        }

    }
}