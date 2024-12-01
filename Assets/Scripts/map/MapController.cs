using System;
using System.Linq;
using helper;
using influence;
using influence.buildings;
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

                    HandleBuilding(x, y, index);
                }
            }

            terrainTilemap.SetTiles(positions, terrain);
            buildingTilemap.SetTiles(positions, building);
        }

        private void HandleBuilding(int x, int y, int tileType)
        {
            var buildingType = TileTypes[_tiles[tileType]].buildingType?.CreateBuildingType();
            if (buildingType == null)
            {
                _buildingController.RemoveBuilding(new Vector2Int(x, y));
            }
            else
            {
                _buildingController.AddBuilding(new Vector2Int(x, y), buildingType);
            }
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
            _tiles[index] = tileTypeIndex;

            terrainTilemap.SetTile(new Vector3Int(x, y), type.terrain);
            buildingTilemap.SetTile(new Vector3Int(x, y), type.building);

            HandleBuilding(x, y, index);

            _gridEvents.MapTileChangedEvent(x, y);
        }

        public void Darken(bool dark)
        {
            terrainTilemap.color = dark ? Color.grey : Color.white;
            buildingTilemap.color = dark ? Color.grey : Color.white;
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

        private T[] FromTileInformation<T>(Func<TileTypeInformation, T> func)
        {
            int layers = EnumUtils.GetLength<Layer>();
            int tileTypes = TileTypes.Length;
            T[] result = new T[layers * tileTypes];

            for (int layer = 0; layer < layers; layer++)
            {
                for (int tileType = 0; tileType < tileTypes; tileType++)
                {
                    result[tileType * layers + layer] = func.Invoke(TileTypes[tileType].ByLayer(layer));
                }
            }

            return result;
        }

        public double[] LossByTileType()
        {
            return FromTileInformation(info => info.loss);
        }

        public double[] LiquidityByTileType()
        {
            return FromTileInformation(info => info.liquidity);
        }
    }
}