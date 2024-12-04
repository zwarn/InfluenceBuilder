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

            _shader = new PropagateShader(width, height, depth, tileTypeCount, liquidity, loss, propagateShader);
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
            var tiles = _mapController.GetTiles();

            var output = _shader.Propagate(values, tiles);
            _grids.SetValues(output);
        }

        public void AddInfluence(Layer layer, int x, int y, double amount)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grids.AddValue(layer, x, y, amount);
            }

            _gridEvents.GridUpdateEvent();
        }

        public double RemoveInfluence(Layer layer, int x, int y, double amount)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                return _grids.RemoveValue(layer, x, y, amount);
            }

            return 0;
        }

        public double GetValue(Layer layer, int x, int y)
        {
            return _grids.GetValue(layer, x, y);
        }

        public double[] GetValues(Layer layer)
        {
            return _grids.GetValues(layer);
        }
    }
}