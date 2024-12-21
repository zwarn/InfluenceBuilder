using System;
using System.Linq;
using helper;
using influence;
using scriptableObjects.map;
using show;
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

        [SerializeField] private Color lightColor = Color.white;
        [SerializeField] private Color darkColor = Color.grey;

        private TileType[] TileTypes => mapCreator.GetTileTypes();
        private int[] _tiles;

        [Inject] private ShowStatusEvents _showStatusEvents;
        [Inject] private GridEvents _gridEvents;
        [Inject] private BuildingController _buildingController;

        private void Awake()
        {
            CreateMap();
            UpdateMapView();
            Darken(false);
        }

        private void OnEnable()
        {
            _showStatusEvents.OnShowTilemap += ShowTilemap;
        }

        private void OnDisable()
        {
            _showStatusEvents.OnShowTilemap -= ShowTilemap;
        }

        private void ShowTilemap(bool show)
        {
            terrainTilemap.gameObject.SetActive(show);
            buildingTilemap.gameObject.SetActive(show);
        }

        private void CreateMap()
        {
            _tiles = mapCreator.CreateMap(width, height);
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
                    terrain[index] = TileTypes[_tiles[index]].terrain;
                    building[index] = TileTypes[_tiles[index]].building;

                    _buildingController.ChangeTile(x, y, TileTypes[_tiles[index]]);
                }
            }

            terrainTilemap.SetTiles(positions, terrain);
            buildingTilemap.SetTiles(positions, building);
        }

        public TileType[] GetTileTypes()
        {
            return TileTypes.ToArray();
        }

        public int[] GetTiles()
        {
            return _tiles.ToArray();
        }

        public void ChangeTile(int x, int y, TileType type)
        {
            int index = y * width + x;
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }

            int tileTypeIndex = Array.IndexOf(TileTypes, type);

            if (_tiles[index] == tileTypeIndex)
            {
                return;
            }

            _tiles[index] = tileTypeIndex;


            terrainTilemap.SetTile(new Vector3Int(x, y), type.terrain);
            buildingTilemap.SetTile(new Vector3Int(x, y), type.building);

            _buildingController.ChangeTile(x, y, type);

            _gridEvents.MapTileChangedEvent(x, y);
        }

        public void Darken(bool dark)
        {
            terrainTilemap.color = dark ? darkColor : lightColor;
            buildingTilemap.color = dark ? darkColor : lightColor;
        }

        public bool IsPointOnMap(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                return true;
            }

            return false;
        }

        public TileType GetTileType(int x, int y)
        {
            int index = y * width + x;
            return TileTypes[_tiles[index]];
        }

        private T[] FromTileType<T>(Func<TileType, Layer, T> func)
        {
            int layers = EnumUtils.GetLength<Layer>();
            int tileTypes = TileTypes.Length;
            T[] result = new T[layers * tileTypes];

            for (int type = 0; type < tileTypes; type++)
            {
                var tileType = TileTypes[type];
                for (int layer = 0; layer < layers; layer++)
                {
                    result[type * layers + layer] = func.Invoke(tileType, (Layer)layer);
                }
            }

            return result;
        }

        public double[] LossByTileType()
        {
            return FromTileType((tileType, layer) =>
                tileType.Properties.TryGetValue(layer, out var property) ? property.loss : 0);
        }

        public double[] LiquidityByTileType()
        {
            return FromTileType((tileType, layer) =>
                tileType.Properties.TryGetValue(layer, out var property) ? property.liquidity : 0);
        }

        public double[] StoreSizeByTileType()
        {
            return FromTileType((tileType, layer) =>
                tileType.Store.TryGetValue(layer, out var store) ? store.storeSize : 0);
        }

        public double[] StoreRateByTileType()
        {
            return FromTileType((tileType, layer) =>
                tileType.Store.TryGetValue(layer, out var store) ? store.storeRate : 0);
        }

        public double[] MinProductionByTileType()
        {
            return FromTileType((tileType, layer) => tileType.Production.TryGetValue(layer, out var production)
                ? production.minHappinessProduction
                : 0);
        }

        public double[] MaxProductionByTileType()
        {
            return FromTileType((tileType, layer) => tileType.Production.TryGetValue(layer, out var production)
                ? production.maxHappinessProduction
                : 0);
        }

        public double[] ConsumptionByTileType()
        {
            return FromTileType((tileType, layer) =>
                tileType.Consumption.TryGetValue(layer, out var consumption) ? consumption.consumption : 0);
        }

        public double[] ConsumptionWeightByTileType()
        {
            return FromTileType((tileType, layer) =>
                tileType.Consumption.TryGetValue(layer, out var consumption) ? consumption.weight : 0);
        }
    }
}