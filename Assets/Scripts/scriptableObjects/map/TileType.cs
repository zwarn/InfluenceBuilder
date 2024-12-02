﻿using System;
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

        public TileTypeInformation ByLayer(int layer)
        {
            return layerInformation.First(info => (int)info.layer == layer);
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