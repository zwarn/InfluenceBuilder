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

        public event Action<InfluenceGrid> GridUpdate;

        private InfluenceGrid _grid;
        private ComputeBuffer _inputBuffer;
        private ComputeBuffer _liquidityBuffer;
        private ComputeBuffer _outputBuffer;

        [Inject] private MapController _mapController;
        [Inject] private InputEvents _inputEvents;

        private int _width;
        private int _height;

        private void Awake()
        {
            _width = _mapController.Width;
            _height = _mapController.Height;

            _grid = new InfluenceGrid(_width, _height);
            _grid.SetValue(0, 0, 10000);

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (x >= 30 && x <= 34 && y >= 15 && y <= 30)
                    {
                        _grid.SetLiquidity(x, y, 0.9);
                    } 
                    else if (x >= 4 && x <= 54 && y >= 20 && y <= 25)
                    {
                        _grid.SetLiquidity(x, y, 0.1);
                    }
                    else
                    {
                        _grid.SetLiquidity(x, y, 0.5);
                    }
                }
            }
        }

        private void OnEnable()
        {
            _inputEvents.OnPerformStepCommand += Tick;

            int width = _mapController.Width;
            int height = _mapController.Height;
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
            GridUpdate?.Invoke(_grid);
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
        }

        public void AddInfluence(int x, int y, int amount)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grid.AddValue(x, y, amount);
            }
        }

        public void RemoveInfluence(int x, int y, int amount)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grid.RemoveValue(x, y, amount);
            }
        }

        public InfluenceGrid GetGrid()
        {
            return _grid;
        }
    }
}