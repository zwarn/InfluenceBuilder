using System;
using System.Linq;
using influence;
using scriptableObjects.building;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace scriptableObjects.map
{
    [CreateAssetMenu(fileName = "TileType", menuName = "map/TileType", order = 0)]
    public class TileType : ScriptableObject
    {
        public TileBase terrain;
        public TileBase building;
        public BuildingTypeSO buildingType;

        public TileTypeInformation[] layerInformation;

        public Layered<double> GetLiquidity()
        {
            var layeredLiquidity = new Layered<double>();
            foreach (var entry in layerInformation)
            {
                layeredLiquidity.AddOrUpdate(entry.layer, entry.liquidity,(a, b) => a + b);
            }
            return layeredLiquidity;
        }
        
        public Layered<double> GetLoss()
        {
            var layeredLoss = new Layered<double>();
            foreach (var entry in layerInformation)
            {
                layeredLoss.AddOrUpdate(entry.layer, entry.loss,(a, b) => a + b);
            }
            return layeredLoss;
        }

    }

    [Serializable]
    public struct TileTypeInformation
    {
        public Layer layer;
        [Range(0, 1)] public double liquidity;
        [Range(0, 1)] public double loss;
    }
}