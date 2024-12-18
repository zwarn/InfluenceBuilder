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
        private static readonly int MinProduction = Shader.PropertyToID("MinProduction");
        private static readonly int MaxProduction = Shader.PropertyToID("MaxProduction");
        private static readonly int Consumption = Shader.PropertyToID("Consumption");
        private static readonly int ConsumptionWeight = Shader.PropertyToID("ConsumptionWeight");
        private static readonly int Store = Shader.PropertyToID("Store");
        private static readonly int Happiness = Shader.PropertyToID("Happiness");

        private ComputeShader _propagateShader;

        private ComputeBuffer _tilesBuffer;

        private ComputeBuffer _storeSizeBuffer;
        private ComputeBuffer _storeRateBuffer;
        private ComputeBuffer _storeBuffer;

        private ComputeBuffer _inputBuffer;
        private ComputeBuffer _liquidityBuffer;
        private ComputeBuffer _outputBuffer;

        private ComputeBuffer _lossBuffer;

        private ComputeBuffer _minProductionBuffer;
        private ComputeBuffer _maxProductionBuffer;
        private ComputeBuffer _consumptionBuffer;
        private ComputeBuffer _consumptionWeightBuffer;
        private ComputeBuffer _happinessBuffer;


        private int _width;
        private int _height;
        private int _depth;

        public PropagateShader(ComputeShader propagateShader, int width, int height, int depth, int tileTypeCount,
            double[] liquidity, double[] loss, double[] storeSize, double[] storeRate, double[] minProduction,
            double[] maxProduction, double[] consumption, double[] consumptionWeight)
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

            _minProductionBuffer = new ComputeBuffer(layersPerTileType, sizeof(double));
            _maxProductionBuffer = new ComputeBuffer(layersPerTileType, sizeof(double));
            _consumptionBuffer = new ComputeBuffer(layersPerTileType, sizeof(double));
            _consumptionWeightBuffer = new ComputeBuffer(layersPerTileType, sizeof(double));
            _happinessBuffer = new ComputeBuffer(tiles, sizeof(double));

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

            _minProductionBuffer.SetData(minProduction);
            _propagateShader.SetBuffer(0, MinProduction, _minProductionBuffer);
            _maxProductionBuffer.SetData(maxProduction);
            _propagateShader.SetBuffer(0, MaxProduction, _maxProductionBuffer);
            _consumptionBuffer.SetData(consumption);
            _propagateShader.SetBuffer(0, Consumption, _consumptionBuffer);
            _consumptionWeightBuffer.SetData(consumptionWeight);
            _propagateShader.SetBuffer(0, ConsumptionWeight, _consumptionWeightBuffer);
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

            _minProductionBuffer.Release();
            _minProductionBuffer = null;
            _maxProductionBuffer.Release();
            _maxProductionBuffer = null;
            _consumptionBuffer.Release();
            _consumptionBuffer = null;
            _consumptionWeightBuffer.Release();
            _consumptionWeightBuffer = null;
            _happinessBuffer.Release();
            _happinessBuffer = null;
        }

        public (double[] output, double[] store, double[] happiness) Propagate(double[] values,
            int[] tiles, double[] happiness, double[] store)
        {
            double[] output = new double[_width * _height * _depth];

            _inputBuffer.SetData(values);
            _tilesBuffer.SetData(tiles);
            _storeBuffer.SetData(store);
            _happinessBuffer.SetData(happiness);

            _propagateShader.SetBuffer(0, Input, _inputBuffer);
            _propagateShader.SetBuffer(0, Tiles, _tilesBuffer);
            _propagateShader.SetBuffer(0, Output, _outputBuffer);
            _propagateShader.SetBuffer(0, Store, _storeBuffer);
            _propagateShader.SetBuffer(0, Happiness, _happinessBuffer);


            int threadGroupsX = Mathf.CeilToInt(_width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(_height / 8.0f);
            int threadGroupsZ = Mathf.CeilToInt(_depth);
            _propagateShader.Dispatch(_propagateShader.FindKernel("CSMain"), threadGroupsX, threadGroupsY,
                threadGroupsZ);

            _outputBuffer.GetData(output);
            _storeBuffer.GetData(store);
            _happinessBuffer.GetData(happiness);

            return (output, store, happiness);
        }
    }
}