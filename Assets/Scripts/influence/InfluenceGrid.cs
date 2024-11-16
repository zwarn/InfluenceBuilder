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
        private NativeArray<double> _grid;
        
        public InfluenceGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new NativeArray<double>(Width * Height, Allocator.Persistent);
        }

        public double GetValue(int x, int y)
        {
            return _grid[y * Width + x];
        }

        public double[] GetValues()
        {
            return _grid.ToArray();
        }
        
        public NativeArray<double> GetNativeArray()
        {
            return _grid;
        }

        public void SetValues(double[] data)
        {
            NativeArray<double> newGrid = new NativeArray<double>(data, Allocator.Persistent);
            _grid.Dispose();
            _grid = newGrid;
        }

        public void SetValue(int x, int y, double value)
        {
            _grid[y * Width + x] = value;
        }

        public void AddValue(int x, int y, double value)
        {
            _grid[y * Width + x] += value;
        }

        public void RemoveValue(int x, int y, double value)
        {
            _grid[y * Width + x] = math.max(_grid[y * Width + x] - value, 0);
        }

        public void Dispose()
        {
            _grid.Dispose();
        }
    }
}