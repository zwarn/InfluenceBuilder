﻿using System;
using System.Linq;
using influence;
using input;
using scriptableObjects.map;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace map
{
    public class MapController : MonoBehaviour
    {
        public int height;
        public int width;

        [SerializeField] private Tilemap terrainTilemap;
        [SerializeField] private Tilemap buildingTilemap;
        [SerializeField] private MapCreator mapCreator;

        private TileType[] _tileTypes;
        private bool _show = true;

        [Inject] private InputEvents _inputEvents;
        [Inject] private GridEvents _gridEvents;

        private void Awake()
        {
            CreateMap();
            UpdateMapView();
            Darken(false);
        }

        private void OnEnable()
        {
            _inputEvents.OnToggleShowTilemap += ToggleShow;
        }

        private void OnDisable()
        {
            _inputEvents.OnToggleShowTilemap -= ToggleShow;
        }

        private void ToggleShow()
        {
            _show = !_show;
            terrainTilemap.gameObject.SetActive(_show);
            buildingTilemap.gameObject.SetActive(_show);
        }

        private void CreateMap()
        {
            _tileTypes = mapCreator.CreateMap(width, height);
        }

        private void UpdateMapView()
        {
            var positions = new Vector3Int[width * height];
            var terrain = new TileBase[width * height];
            var building = new TileBase[width * height];

            terrainTilemap.ClearAllTiles();
            buildingTilemap.ClearAllTiles();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var index = y * height + x;
                    positions[index] = new Vector3Int(x, y);
                    terrain[index] = _tileTypes[index].terrain;
                    building[index] = _tileTypes[index].building;
                }
            }

            terrainTilemap.SetTiles(positions, terrain);
            buildingTilemap.SetTiles(positions, building);
        }

        public double[] GetLiquidity()
        {
            return _tileTypes.Select(tile => tile.liquidity).ToArray();
        }
        
        public double[] GetLoss()
        {
            return _tileTypes.Select(tile => tile.loss).ToArray();
        }

        public double GetLiquidity(int x, int y)
        {
            return _tileTypes[y * width + x].liquidity;
        }

        public TileType[] GetTileTypes()
        {
            return _tileTypes.ToArray();
        }

        public void ChangeTile(int x, int y, TileType type)
        {
            int index = y * width + x;
            if (index < 0 || index > _tileTypes.Length)
            {
                return;
            }

            _tileTypes[index] = type;

            terrainTilemap.SetTile(new Vector3Int(x,y), type.terrain);
            buildingTilemap.SetTile(new Vector3Int(x,y), type.building);

            _gridEvents.MapTileChangedEvent(x, y);
        }

        public void Darken(bool dark)
        {
            terrainTilemap.color = dark ? Color.grey : Color.white;
            buildingTilemap.color = dark ? Color.grey : Color.white;
        }
    }
}