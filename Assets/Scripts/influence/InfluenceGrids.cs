using System;
using scriptableObjects.map;

namespace influence
{
    public class InfluenceGrids
    {
        private InfluenceGrid[] _grids;
        private int _width;
        private int _height;
        private int _depth;

        public InfluenceGrids(int width, int height)
        {
            _width = width;
            _height = height;
            _depth = Enum.GetValues(typeof(Layer)).Length;
            _grids = new InfluenceGrid[_depth];
            for (var i = 0; i < _grids.Length; i++)
            {
                _grids[i] = new InfluenceGrid(width, height);
            }
        }

        public void ApplyProduction(int[] tiles, TileType[] tileTypes)
        {
            for (var index = 0; index < tiles.Length; index++)
            {
                foreach (var info in tileTypes[tiles[index]].layerInformation)
                {
                    _grids[(int)info.layer].AddValue(index, info.production);
                }
            }
        }

        public void Dispose()
        {
            for (var i = 0; i < _grids.Length; i++)
            {
                _grids[i].Dispose();
            }
        }

        public void AddValue(Layer layer, int x, int y, double amount)
        {
            _grids[(int)layer].AddValue(x, y, amount);
        }

        public void RemoveValue(Layer layer, int x, int y, double amount)
        {
            _grids[(int)layer].RemoveValue(x, y, amount);
        }

        public double GetValue(Layer layer, int x, int y)
        {
            return _grids[(int)layer].GetValue(x, y);
        }

        public double[] GetValues(Layer layer)
        {
            return _grids[(int)layer].GetValues();
        }

        public double[] GetValues()
        {
            double[] result = new double[_depth * _width * _height];

            int offset = 0;
            foreach (var grid in _grids)
            {
                var values = grid.GetValues();
                Array.Copy(values, 0, result, offset, values.Length);
                offset += values.Length;
            }

            return result;
        }

        public void SetValues(double[] values)
        {
            int size = _width * _height;

            for (int layer = 0; layer < _depth; layer++)
            {
                double[] chunk = new double[size];
                Array.Copy(values, layer * size, chunk, 0, size);

                _grids[layer].SetValues(chunk);
            }
        }
    }
}