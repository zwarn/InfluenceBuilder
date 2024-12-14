using System;
using influence;
using ModestTree;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace scriptableObjects.map
{
    [CreateAssetMenu(fileName = "TileType", menuName = "map/TileType", order = 0)]
    public class TileType : ScriptableObject
    {
        public TileBase terrain;
        public TileBase building;

        public TileTypeInformation[] layerInformation;
        public ProductionInformation[] productionInformation;
        public StoreInformation[] storeInformation;
        public int cooldown;

        public Layered<double> GetLiquidity()
        {
            var layeredLiquidity = new Layered<double>();
            foreach (var entry in layerInformation)
            {
                layeredLiquidity.AddOrUpdate(entry.layer, entry.liquidity, (a, b) => a + b);
            }

            return layeredLiquidity;
        }

        public Layered<double> GetLoss()
        {
            var layeredLoss = new Layered<double>();
            foreach (var entry in layerInformation)
            {
                layeredLoss.AddOrUpdate(entry.layer, entry.loss, (a, b) => a + b);
            }

            return layeredLoss;
        }

        public bool HasProduction()
        {
            return !productionInformation.IsEmpty();
        }

        public Layered<ProductionInformation> GetProductionInformation()
        {
            var layeredProduction = new Layered<ProductionInformation>();
            foreach (var entry in productionInformation)
            {
                layeredProduction.AddOrUpdate(entry.layer, entry, Layered<ProductionInformation>.Override());
            }

            return layeredProduction;
        }
        
        public Layered<StoreInformation> GetStoreInformation()
        {
            var layeredStorage = new Layered<StoreInformation>();
            foreach (var entry in storeInformation)
            {
                layeredStorage.AddOrUpdate(entry.layer, entry, Layered<StoreInformation>.Override());
            }

            return layeredStorage;
        }
    }

    [Serializable]
    public struct TileTypeInformation
    {
        public Layer layer;
        [Range(0, 1)] public double liquidity;
        [Range(0, 1)] public double loss;
    }

    [Serializable]
    public struct ProductionInformation
    {
        public Layer layer;
        public double production;
        public double consumption;
    }

    [Serializable]
    public struct StoreInformation
    {
        public Layer layer;
        public double storeSize;
        public double storeRate;
    }
}