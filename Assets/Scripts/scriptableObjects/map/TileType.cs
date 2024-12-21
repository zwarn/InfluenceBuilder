using System;
using System.Collections.Generic;
using influence;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace scriptableObjects.map
{
    [CreateAssetMenu(fileName = "TileType", menuName = "map/TileType", order = 0)]
    public class TileType : SerializedScriptableObject
    {
        public TileBase terrain;
        public TileBase building;

        public bool isBuilding;
        public readonly Dictionary<Layer, Properties> Properties = new();
        public readonly Dictionary<Layer, Production> Production = new();
        public readonly Dictionary<Layer, Consumption> Consumption = new();
        public readonly Dictionary<Layer, Store> Store = new();

        public Layered<double> GetLiquidity()
        {
            var layeredLiquidity = new Layered<double>();
            foreach (var entry in Properties)
            {
                layeredLiquidity.AddOrUpdate(entry.Key, entry.Value.liquidity, (a, b) => a + b);
            }

            return layeredLiquidity;
        }

        public Layered<double> GetLoss()
        {
            var layeredLoss = new Layered<double>();
            foreach (var entry in Properties)
            {
                layeredLoss.AddOrUpdate(entry.Key, entry.Value.loss, (a, b) => a + b);
            }

            return layeredLoss;
        }

        public Layered<Production> GetProduction()
        {
            var layeredProduction = new Layered<Production>();
            foreach (var entry in Production)
            {
                layeredProduction.AddOrUpdate(entry.Key, entry.Value, Layered<Production>.Override());
            }

            return layeredProduction;
        }

        public Layered<Consumption> GetConsumption()
        {
            var layeredConsumption = new Layered<Consumption>();
            foreach (var entry in Consumption)
            {
                layeredConsumption.AddOrUpdate(entry.Key, entry.Value, Layered<Consumption>.Override());
            }

            return layeredConsumption;
        }

        public Layered<Store> GetStoreInformation()
        {
            var layeredStorage = new Layered<Store>();
            foreach (var entry in Store)
            {
                layeredStorage.AddOrUpdate(entry.Key, entry.Value, Layered<Store>.Override());
            }

            return layeredStorage;
        }
    }

    [Serializable]
    public struct Properties
    {
        [Range(0, 1)] public double liquidity;
        [Range(0, 1)] public double loss;
    }

    [Serializable]
    public struct Production
    {
        public double minHappinessProduction;
        public double maxHappinessProduction;
    }

    [Serializable]
    public struct Consumption
    {
        public double consumption;
        public double weight;
    }

    [Serializable]
    public struct Store
    {
        public double storeSize;
        public double storeRate;
    }
}