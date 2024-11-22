using System;
using scriptableObjects.map;
using Unity.Collections;

namespace influence
{
    public class InfluenceGrids
    {
        private InfluenceGrid[] _grids;

        public InfluenceGrids(int width, int height)
        {
            _grids = new InfluenceGrid[Enum.GetValues(typeof(Layer)).Length];
            for (var i = 0; i < _grids.Length; i++)
            {
                _grids[i] = new InfluenceGrid(width, height);
            }
        }

        public void SetLiquidity(TileType[] tileTypes)
        {
            for (var index = 0; index < tileTypes.Length; index++)
            {
                foreach (var info in tileTypes[index].layerInformation)
                {
                    _grids[(int)info.layer].SetLiquidity(index, info.liquidity);
                }
            }
        }


        public void ApplyLoss(TileType[] tileTypes)
        {
            for (var index = 0; index < tileTypes.Length; index++)
            {
                foreach (var info in tileTypes[index].layerInformation)
                {
                    _grids[(int)info.layer].RemoveValue(index, info.loss);
                }
            }
        }


        public void ApplyProduction(TileType[] tileTypes)
        {
            for (var index = 0; index < tileTypes.Length; index++)
            {
                foreach (var info in tileTypes[index].layerInformation)
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

        public void AddValue(Layer layer, int index, double amount)
        {
            _grids[(int)layer].AddValue(index, amount);
        }

        public NativeArray<double> GetValuesArray(Layer layer)
        {
            return _grids[(int)layer].GetValuesArray();
        }

        public NativeArray<double> GetLiquidityArray(Layer layer)
        {
            return _grids[(int)layer].GetLiquidityArray();
        }

        public void SetValues(Layer layer, double[] values)
        {
            _grids[(int)layer].SetValues(values);
        }

        public void AddValue(Layer layer, int x, int y, double amount)
        {
            _grids[(int)layer].AddValue(x, y, amount);
        }

        public void RemoveValue(Layer layer, int x, int y, double amount)
        {
            _grids[(int)layer].RemoveValue(x, y, amount);
        }

        public void UpdateTile(int x, int y, TileType tileType)
        {
            foreach (var info in tileType.layerInformation)
            {
                _grids[(int)info.layer].SetLiquidity(x, y, info.liquidity);
            }
        }

        public double GetValue(Layer layer, int x, int y)
        {
            return _grids[(int)layer].GetValue(x, y);
        }

        public double[] GetValues(Layer layer)
        {
            return _grids[(int)layer].GetValues();
        }
    }
}