using UnityEngine;

namespace influence
{
    public class PropagateShader
    {
        private static readonly int Input = Shader.PropertyToID("Input");
        private static readonly int Output = Shader.PropertyToID("Output");
        private static readonly int Width = Shader.PropertyToID("Width");
        private static readonly int Height = Shader.PropertyToID("Height");
        private static readonly int Depth = Shader.PropertyToID("Depth");
        private static readonly int Tiles = Shader.PropertyToID("Tiles");
        private static readonly int Liquidity = Shader.PropertyToID("Liquidity");
        private static readonly int Loss = Shader.PropertyToID("Loss");

        private ComputeShader _propagateShader;
        private ComputeBuffer _inputBuffer;
        private ComputeBuffer _tilesBuffer;
        private ComputeBuffer _liquidityBuffer;
        private ComputeBuffer _lossBuffer;
        private ComputeBuffer _outputBuffer;

        private int _width;
        private int _height;
        private int _depth;

        public PropagateShader(int width, int height, int depth, int tileTypeCount, double[] liquidity, double[] loss,
            ComputeShader propagateShader)
        {
            _width = width;
            _height = height;
            _depth = depth;
            _propagateShader = propagateShader;

            int size = _width * _height * _depth;
            _inputBuffer = new ComputeBuffer(size, sizeof(double));
            _tilesBuffer = new ComputeBuffer(width * height, sizeof(int));
            _liquidityBuffer = new ComputeBuffer(depth * tileTypeCount, sizeof(double));
            _lossBuffer = new ComputeBuffer(depth * tileTypeCount, sizeof(double));
            _outputBuffer = new ComputeBuffer(size, sizeof(double));

            _propagateShader.SetInt(Width, width);
            _propagateShader.SetInt(Height, height);
            _propagateShader.SetInt(Depth, depth);

            _liquidityBuffer.SetData(liquidity);
            _propagateShader.SetBuffer(0, Liquidity, _liquidityBuffer);
            _lossBuffer.SetData(loss);
            _propagateShader.SetBuffer(0, Loss, _lossBuffer);
        }

        public void Dispose()
        {
            _inputBuffer.Release();
            _inputBuffer = null;
            _tilesBuffer.Release();
            _tilesBuffer = null;
            _liquidityBuffer.Release();
            _liquidityBuffer = null;
            _lossBuffer.Release();
            _lossBuffer = null;
            _outputBuffer.Release();
            _outputBuffer = null;
        }

        public double[] Propagate(double[] values, int[] tiles)
        {
            double[] output = new double[_width * _height * _depth];

            _inputBuffer.SetData(values);
            _tilesBuffer.SetData(tiles);

            _propagateShader.SetBuffer(0, Input, _inputBuffer);
            _propagateShader.SetBuffer(0, Tiles, _tilesBuffer);
            _propagateShader.SetBuffer(0, Output, _outputBuffer);


            int threadGroupsX = Mathf.CeilToInt(_width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(_height / 8.0f);
            int threadGroupsZ = Mathf.CeilToInt(_depth);
            _propagateShader.Dispatch(0, threadGroupsX, threadGroupsY, threadGroupsZ);

            _outputBuffer.GetData(output);

            return output;
        }
    }
}