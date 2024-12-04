using System;
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
        private static readonly int StoreSize = Shader.PropertyToID("StoreSize");
        private static readonly int StoreRate = Shader.PropertyToID("StoreRate");
        private static readonly int Production = Shader.PropertyToID("Production");
        private static readonly int Consumption = Shader.PropertyToID("Consumption");
        private static readonly int Cooldown = Shader.PropertyToID("Cooldown");
        private static readonly int Timer = Shader.PropertyToID("Timer");
        private static readonly int Store = Shader.PropertyToID("Store");

        private ComputeShader _propagateShader;

        private ComputeBuffer _tilesBuffer;

        private ComputeBuffer _storeSizeBuffer;
        private ComputeBuffer _storeRateBuffer;
        private ComputeBuffer _storeBuffer;

        private ComputeBuffer _inputBuffer;
        private ComputeBuffer _liquidityBuffer;
        private ComputeBuffer _outputBuffer;

        private ComputeBuffer _lossBuffer;

        private ComputeBuffer _productionBuffer;
        private ComputeBuffer _consumptionBuffer;
        private ComputeBuffer _cooldownBuffer;
        private ComputeBuffer _timerBuffer;


        private int _width;
        private int _height;
        private int _depth;

        public PropagateShader(ComputeShader propagateShader, int width, int height, int depth, int tileTypeCount,
            double[] liquidity, double[] loss, double[] storeSize, double[] storeRate, double[] production,
            double[] consumption, int[] cooldown)
        {
            _width = width;
            _height = height;
            _depth = depth;
            _propagateShader = propagateShader;

            int layersPerTile = _width * _height * _depth;
            int tiles = width * height;
            int layersPerTileType = depth * tileTypeCount;

            _tilesBuffer = new ComputeBuffer(tiles, sizeof(int));

            _storeSizeBuffer = new ComputeBuffer(layersPerTileType, sizeof(double));
            _storeRateBuffer = new ComputeBuffer(layersPerTileType, sizeof(double));
            _storeBuffer = new ComputeBuffer(layersPerTile, sizeof(double));

            _inputBuffer = new ComputeBuffer(layersPerTile, sizeof(double));
            _liquidityBuffer = new ComputeBuffer(layersPerTileType, sizeof(double));
            _outputBuffer = new ComputeBuffer(layersPerTile, sizeof(double));

            _lossBuffer = new ComputeBuffer(layersPerTileType, sizeof(double));

            _productionBuffer = new ComputeBuffer(layersPerTileType, sizeof(double));
            _consumptionBuffer = new ComputeBuffer(layersPerTileType, sizeof(double));
            _cooldownBuffer = new ComputeBuffer(tileTypeCount, sizeof(int));
            _timerBuffer = new ComputeBuffer(tiles, sizeof(int));

            _propagateShader.SetInt(Width, width);
            _propagateShader.SetInt(Height, height);
            _propagateShader.SetInt(Depth, depth);

            _storeSizeBuffer.SetData(storeSize);
            _propagateShader.SetBuffer(0, StoreSize, _storeSizeBuffer);
            _storeRateBuffer.SetData(storeRate);
            _propagateShader.SetBuffer(0, StoreRate, _storeRateBuffer);

            _liquidityBuffer.SetData(liquidity);
            _propagateShader.SetBuffer(0, Liquidity, _liquidityBuffer);

            _lossBuffer.SetData(loss);
            _propagateShader.SetBuffer(0, Loss, _lossBuffer);

            _productionBuffer.SetData(production);
            _propagateShader.SetBuffer(0, Production, _productionBuffer);
            _consumptionBuffer.SetData(consumption);
            _propagateShader.SetBuffer(0, Consumption, _consumptionBuffer);
            _cooldownBuffer.SetData(cooldown);
            _propagateShader.SetBuffer(0, Cooldown, _cooldownBuffer);
        }

        public void Dispose()
        {
            _tilesBuffer.Release();
            _tilesBuffer = null;

            _storeSizeBuffer.Release();
            _storeSizeBuffer = null;

            _storeRateBuffer.Release();
            _storeRateBuffer = null;
            _storeBuffer.Release();
            _storeBuffer = null;

            _inputBuffer.Release();
            _inputBuffer = null;
            _liquidityBuffer.Release();
            _liquidityBuffer = null;
            _outputBuffer.Release();
            _outputBuffer = null;

            _lossBuffer.Release();
            _lossBuffer = null;

            _productionBuffer.Release();
            _productionBuffer = null;
            _consumptionBuffer.Release();
            _consumptionBuffer = null;
            _cooldownBuffer.Release();
            _cooldownBuffer = null;
            _timerBuffer.Release();
            _timerBuffer = null;
        }

        public (double[] output, double[] store, int[] timer) Propagate(double[] values, int[] tiles, int[] timer,
            double[] store)
        {
            double[] output = new double[_width * _height * _depth];

            _inputBuffer.SetData(values);
            _tilesBuffer.SetData(tiles);
            _storeBuffer.SetData(store);
            _timerBuffer.SetData(timer);

            _propagateShader.SetBuffer(0, Input, _inputBuffer);
            _propagateShader.SetBuffer(0, Tiles, _tilesBuffer);
            _propagateShader.SetBuffer(0, Output, _outputBuffer);
            _propagateShader.SetBuffer(0, Store, _storeBuffer);
            _propagateShader.SetBuffer(0, Timer, _timerBuffer);


            int threadGroupsX = Mathf.CeilToInt(_width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(_height / 8.0f);
            int threadGroupsZ = Mathf.CeilToInt(_depth);
            _propagateShader.Dispatch(0, threadGroupsX, threadGroupsY, threadGroupsZ);

            _outputBuffer.GetData(output);
            _storeBuffer.GetData(store);
            _timerBuffer.GetData(timer);

            return (output, store, timer);
        }
    }
}