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