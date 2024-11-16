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

        public event Action<InfluenceGrid> GridUpdate;

        private InfluenceGrid _grid;
        private ComputeBuffer _inputBuffer;
        private ComputeBuffer _outputBuffer;

        [Inject] private MapController _mapController;
        [Inject] private InputEvents _inputEvents;

        private int _width;
        private int _height;


        private void Start()
        {
            _width = _mapController.Width;
            _height = _mapController.Height;

            _grid = new InfluenceGrid(_width, _height);
            _grid.SetValue(0, 0, 1000);
            _grid.SetValue(1, 1, 1000);
            _grid.SetValue(2, 2, 1000);
            _grid.SetValue(3, 3, 1000);
            _grid.SetValue(4, 4, 1000);
            _grid.SetValue(5, 5, 1000);
            _grid.SetValue(6, 6, 1000);
            _grid.SetValue(7, 7, 1000);
            _grid.SetValue(8, 8, 1000);
            _grid.SetValue(9, 9, 1000);
            _grid.SetValue(10, 10, 1000);
        }

        private void OnEnable()
        {
            _inputEvents.OnPerformStepCommand += Tick;
            _inputEvents.OnAddInfluenceCommand += AddInfluence;
            _inputEvents.OnRemoveInfluenceCommand += RemoveInfluence;

            int width = _mapController.Width;
            int height = _mapController.Height;
            int size = width * height;
            _inputBuffer = new ComputeBuffer(size, sizeof(double));
            _outputBuffer = new ComputeBuffer(size, sizeof(double));

            propagateShader.SetInt(Width, width);
            propagateShader.SetInt(Height, height);
            propagateShader.SetFloat(Liquidity, 0.5f);
        }

        private void OnDisable()
        {
            _inputEvents.OnPerformStepCommand -= Tick;
            _inputEvents.OnAddInfluenceCommand -= AddInfluence;
            _inputEvents.OnRemoveInfluenceCommand -= RemoveInfluence;

            _inputBuffer.Release();
            _inputBuffer = null;
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

            var data = _grid.GetNativeArray();
            _inputBuffer.SetData(data);
            propagateShader.SetBuffer(0, Input, _inputBuffer);
            propagateShader.SetBuffer(0, Output, _outputBuffer);

            int threadGroupsX = Mathf.CeilToInt(_width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(_height / 8.0f);
            propagateShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

            _outputBuffer.GetData(output);

            _grid.SetValues(output);
        }

        public void AddInfluence(Vector2Int pos)
        {
            var x = pos.x;
            var y = pos.y;
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grid.AddValue(x, y, 1);
            }
        }

        public void RemoveInfluence(Vector2Int pos)
        {
            var x = pos.x;
            var y = pos.y;
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grid.RemoveValue(x, y, 1);
            }
        }

        public InfluenceGrid GetGrid()
        {
            return _grid;
        }
    }
}