using System;
using influence;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace scriptableObjects.map
{
    [CreateAssetMenu(fileName = "TileType", menuName = "map/TileType", order = 0)]
    public class TileType : ScriptableObject
    {
        public TileBase terrain;
        public TileBase building;

        public LayeredProperties[] properties;
        public Production production;
        public LayeredConsumption[] consumptionInformation;
        public LayeredStore[] storeInformation;

        public Layered<double> GetLiquidity()
        {
            var layeredLiquidity = new Layered<double>();
            foreach (var entry in properties)
            {
                layeredLiquidity.AddOrUpdate(entry.layer, entry.liquidity, (a, b) => a + b);
            }

            return layeredLiquidity;
        }

        public Layered<double> GetLoss()
        {
            var layeredLoss = new Layered<double>();
            foreach (var entry in properties)
            {
                layeredLoss.AddOrUpdate(entry.layer, entry.loss, (a, b) => a + b);
            }

            return layeredLoss;
        }

        public Layered<LayeredProduction> GetProductionInformation()
        {
            var layeredProduction = new Layered<LayeredProduction>();
            foreach (var entry in production.layeredProduction)
            {
                layeredProduction.AddOrUpdate(entry.layer, entry, Layered<LayeredProduction>.Override());
            }

            return layeredProduction;
        }        
        public Layered<LayeredConsumption> GetConsumptionInformation()
        {
            var layeredConsumption = new Layered<LayeredConsumption>();
            foreach (var entry in consumptionInformation)
            {
                layeredConsumption.AddOrUpdate(entry.layer, entry, Layered<LayeredConsumption>.Override());
            }

            return layeredConsumption;
        }
        
        public Layered<LayeredStore> GetStoreInformation()
        {
            var layeredStorage = new Layered<LayeredStore>();
            foreach (var entry in storeInformation)
            {
                layeredStorage.AddOrUpdate(entry.layer, entry, Layered<LayeredStore>.Override());
            }

            return layeredStorage;
        }
    }

    [Serializable]
    public struct LayeredProperties
    {
        public Layer layer;
        [Range(0, 1)] public double liquidity;
        [Range(0, 1)] public double loss;
    }

    [Serializable]
    public struct Production
    {
        public LayeredProduction[] layeredProduction;
    }
    
    [Serializable]
    public struct LayeredProduction
    {
        public Layer layer;
        public double minHappinessProduction;
        public double maxHappinessProduction;
    }

    [Serializable]
    public struct LayeredConsumption
    {
        public Layer layer;
        public double consumption;
        public double weight;
    }

    [Serializable]
    public struct LayeredStore
    {
        public Layer layer;
        public double storeSize;
        public double storeRate;
    }
}