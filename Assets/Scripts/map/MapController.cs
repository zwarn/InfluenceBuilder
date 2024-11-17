using System;
using System.Linq;
using scriptableObjects.map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace map
{
    public class MapController : MonoBehaviour
    {
        public int height;
        public int width;

        [SerializeField] private Tilemap tilemap;
        [SerializeField] private MapCreator mapCreator;

        private TileType[] _tileTypes;

        private void Awake()
        {
            CreateMap();
            UpdateMapView();
        }

        private void CreateMap()
        {
            _tileTypes = mapCreator.CreateMap(width, height);
        }

        private void UpdateMapView()
        {
            var positions = new Vector3Int[width * height];
            var tileBase = new TileBase[width * height];

            tilemap.ClearAllTiles();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var index = y * height + x;
                    positions[index] = new Vector3Int(x, y);
                    tileBase[index] = _tileTypes[index].tile;
                }
            }

            tilemap.SetTiles(positions, tileBase);
        }

        public double[] GetLiquidity()
        {
            return _tileTypes.Select(tile => tile.liquidity).ToArray();
        }
    }
}