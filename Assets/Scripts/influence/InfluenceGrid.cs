using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace influence
{
    [BurstCompile]
    public struct InfluenceGrid
    {
        private const double Liquidity = 0.5;

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
            _liquidity[y * Width + x] = value;
        }

        public void AddValue(int x, int y, double value)
        {
            _values[y * Width + x] += value;
        }

        public void RemoveValue(int x, int y, double value)
        {
            _values[y * Width + x] = math.max(_values[y * Width + x] - value, 0);
        }

        public void Dispose()
        {
            _values.Dispose();
            _liquidity.Dispose();
        }
    }
}