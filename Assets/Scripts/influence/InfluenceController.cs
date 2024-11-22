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
        private static readonly int Input = Shader.PropertyToID("Input");
        private static readonly int Output = Shader.PropertyToID("Output");
        private static readonly int Width = Shader.PropertyToID("Width");
        private static readonly int Height = Shader.PropertyToID("Height");
        private static readonly int Liquidity = Shader.PropertyToID("Liquidity");

        [SerializeField] private ComputeShader propagateShader;

        private InfluenceGrids _grids;
        private ComputeBuffer _inputBuffer;
        private ComputeBuffer _liquidityBuffer;
        private ComputeBuffer _outputBuffer;

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
            int size = width * height;
            _inputBuffer = new ComputeBuffer(size, sizeof(double));
            _liquidityBuffer = new ComputeBuffer(size, sizeof(double));
            _outputBuffer = new ComputeBuffer(size, sizeof(double));

            propagateShader.SetInt(Width, width);
            propagateShader.SetInt(Height, height);
        }

        private void OnDisable()
        {
            _inputEvents.OnPerformStepCommand -= Tick;
            _gridEvents.OnMapTileChanged -= MapTileChanged;

            _inputBuffer.Release();
            _inputBuffer = null;
            _liquidityBuffer.Release();
            _liquidityBuffer = null;
            _outputBuffer.Release();
            _outputBuffer = null;

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
            double[] output = new double[_width * _height];

            var values = _grids.GetValuesArray(layer);
            _inputBuffer.SetData(values);
            var liquidity = _grids.GetLiquidityArray(layer);
            _liquidityBuffer.SetData(liquidity);
            propagateShader.SetBuffer(0, Input, _inputBuffer);
            propagateShader.SetBuffer(0, Liquidity, _liquidityBuffer);
            propagateShader.SetBuffer(0, Output, _outputBuffer);


            int threadGroupsX = Mathf.CeilToInt(_width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(_height / 8.0f);
            propagateShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

            _outputBuffer.GetData(output);

            _grids.SetValues(layer, output);
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