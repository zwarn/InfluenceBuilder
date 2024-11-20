using UnityEngine;
using UnityEngine.Tilemaps;

namespace scriptableObjects.map
{
    [CreateAssetMenu(fileName = "TileType", menuName = "map/TileType", order = 0)]
    public class TileType : ScriptableObject
    {
        public TileBase terrain;
        public TileBase building;
        [Range(0, 1)] public double liquidity;
        [Range(0, 1)] public double loss;
    }
}