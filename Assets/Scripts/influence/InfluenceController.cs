using System;
using input;
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
        [Inject] private InputEvents _inputEvents;
        [Inject] private GridEvents _gridEvents;

        private int _width;
        private int _height;

        private void Awake()
        {
            _width = _mapController.width;
            _height = _mapController.height;

            _grids = new InfluenceGrids(_width, _height);
        }

        private void Start()
        {
            _grids.SetLiquidity(_mapController.GetTileTypes());
        }

        private void OnEnable()
        {
            _inputEvents.OnPerformStepCommand += Tick;
            _gridEvents.OnMapTileChanged += MapTileChanged;

            int width = _mapController.width;
            int height = _mapController.height;
            int depth = Enum.GetValues(typeof(Layer)).Length;

            _shader = new PropagateShader(width, height, depth, propagateShader);

        }

        private void OnDisable()
        {
            _inputEvents.OnPerformStepCommand -= Tick;
            _gridEvents.OnMapTileChanged -= MapTileChanged;

            _shader.Dispose();
            _grids.Dispose();
        }

        public void Tick()
        {
            Produce();
            Propagate();
            ApplyLoss();
            _gridEvents.GridUpdateEvent();
        }

        private void Produce()
        {
            var tileTypes = _mapController.GetTileTypes();

            _grids.ApplyProduction(tileTypes);
        }

        private void Propagate()
        {
            foreach (Layer layer in Enum.GetValues(typeof(Layer)))
            {
                Propagate(layer);
            }
        }

        private void Propagate(Layer layer)
        {
            var values = _grids.GetValues();
            var liquidity = _grids.GetLiquidity();

            var output = _shader.Propagate(values, liquidity);
            _grids.SetValues(output);
        }

        private void ApplyLoss()
        {
            var tileTypes = _mapController.GetTileTypes();
            _grids.ApplyLoss(tileTypes);
        }

        public void AddInfluence(Layer layer, int x, int y, int amount)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grids.AddValue(layer, x, y, amount);
            }

            _gridEvents.GridUpdateEvent();
        }

        public void RemoveInfluence(Layer layer, int x, int y, int amount)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grids.RemoveValue(layer, x, y, amount);
            }

            _gridEvents.GridUpdateEvent();
        }

        private void MapTileChanged(Vector2Int pos)
        {
            var tileType = _mapController.GetTileType(pos.x, pos.y);
            _grids.UpdateTile(pos.x, pos.y, tileType);
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