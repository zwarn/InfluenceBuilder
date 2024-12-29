using System;
using map;
using Unity.Burst;
using UnityEngine;
using Zenject;

namespace influence
{
    [BurstCompile]
    public class InfluenceController : MonoBehaviour
    {
        [SerializeField] private ComputeShader propagateShader;

        private InfluenceGrids _grids;
        private PropagateShader _shader;

        [Inject] private MapController _mapController;
        [Inject] private GridEvents _gridEvents;

        private int _width;
        private int _height;

        private void Awake()
        {
            _width = _mapController.width;
            _height = _mapController.height;

            _grids = new InfluenceGrids(_width, _height);
        }

        private void OnEnable()
        {
            int width = _mapController.width;
            int height = _mapController.height;
            int depth = Enum.GetValues(typeof(Layer)).Length;
            int tileTypeCount = _mapController.GetTileTypes().Length;

            var liquidity = _mapController.LiquidityByTileType();
            var loss = _mapController.LossByTileType();
            var storeSize = _mapController.StoreSizeByTileType();
            var storeRate = _mapController.StoreRateByTileType();
            var minProduction = _mapController.MinProductionByTileType();
            var maxProduction = _mapController.MaxProductionByTileType();
            var consumption = _mapController.ConsumptionByTileType();
            var consumptionWeight = _mapController.ConsumptionWeightByTileType();
            var productionWeight = _mapController.ConsumptionWeightByTileType();

            _shader = new PropagateShader(propagateShader, width, height, depth, tileTypeCount, liquidity, loss,
                storeSize, storeRate, minProduction, maxProduction, consumption, consumptionWeight);
        }

        private void OnDisable()
        {
            _shader.Dispose();
            _grids.Dispose();
        }

        public void Step()
        {
            Propagate();
        }

        private void Propagate()
        {
            var values = _grids.GetValues();
            var store = _grids.GetStore();
            var tiles = _mapController.GetTiles();
            var happiness = _grids.GetHappiness();

            var (output, newStore, newHappiness) = _shader.Propagate(values, tiles, happiness, store);
            _grids.SetValues(output);
            _grids.SetStore(newStore);
            _grids.SetHappiness(newHappiness);
        }

        public void AddInfluence(Layer layer, int x, int y, double amount)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grids.AddValue(layer, x, y, amount);
            }

            _gridEvents.GridUpdateEvent();
        }

        public void MinusInfluence(Layer layer, int x, int y, double amount)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grids.MinusValue(layer, x, y, amount);
            }
            
            _gridEvents.GridUpdateEvent();
        }

        public double GetValue(Layer layer, int x, int y)
        {
            return _grids.GetValue(layer, x, y);
        }

        public double[] GetValues(Layer layer)
        {
            return _grids.GetValues(layer);
        }

        public double GetHappiness(int x, int y)
        {
            return _grids.GetHappiness(x, y);
        }

        public double[] GetHappiness()
        {
            return _grids.GetHappiness();
        }
        public double GetStore(Layer layer, int x, int y)
        {
            return _grids.GetStore(layer, x, y);
        }

        public double RemoveStore(Layer layer, int x, int y, double amount)
        {
            return _grids.RemoveStore(layer, x, y, amount);
        }
    }
}