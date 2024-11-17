using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace map
{
    public class MapController : MonoBehaviour
    {
        public int Height;
        public int Width;

        [SerializeField] private Tilemap _tilemap;

        [SerializeField] private List<TileBase> _tiles;

        private void Start()
        {
            CreateMap();
        }

        private void CreateMap()
        {
            _tilemap.ClearAllTiles();

            var positions = new Vector3Int[Width * Height];
            var tileBase = new TileBase[Width * Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var index = y * Height + x;
                    positions[index] = new Vector3Int(x, y);
                    tileBase[index] = _tiles[Random.Range(0, _tiles.Count)];
                }
            }

            _tilemap.SetTiles(positions, tileBase);
        }
    }
}