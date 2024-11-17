using System;
using System.Linq;
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

        private InfluenceGrid _grid;
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

            _grid = new InfluenceGrid(_width, _height);
            _grid.SetValue(0, 0, 10000);
        }

        private void Start()
        {
            _grid.SetLiquidity(_mapController.GetLiquidity());
        }

        private void OnEnable()
        {
            _inputEvents.OnPerformStepCommand += Tick;

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

            _inputBuffer.Release();
            _inputBuffer = null;
            _liquidityBuffer.Release();
            _liquidityBuffer = null;
            _outputBuffer.Release();
            _outputBuffer = null;

            _grid.Dispose();
        }

        public void Tick()
        {
            Propagate();
        }

        private void Propagate()
        {
            double[] output = new double[_width * _height];

            var values = _grid.GetValuesArray();
            _inputBuffer.SetData(values);
            var liquidity = _grid.GetLiquidityArray();
            _liquidityBuffer.SetData(liquidity);
            propagateShader.SetBuffer(0, Input, _inputBuffer);
            propagateShader.SetBuffer(0, Liquidity, _liquidityBuffer);
            propagateShader.SetBuffer(0, Output, _outputBuffer);


            int threadGroupsX = Mathf.CeilToInt(_width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(_height / 8.0f);
            propagateShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

            _outputBuffer.GetData(output);

            _grid.SetValues(output);
            _gridEvents.GridUpdateEvent();
        }

        public void AddInfluence(int x, int y, int amount)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grid.AddValue(x, y, amount);
            }

            _gridEvents.GridUpdateEvent();
        }

        public void RemoveInfluence(int x, int y, int amount)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grid.RemoveValue(x, y, amount);
            }

            _gridEvents.GridUpdateEvent();
        }

        public InfluenceGrid GetGrid()
        {
            return _grid;
        }
    }
}