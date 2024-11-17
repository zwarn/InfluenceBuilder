using System;
using System.Linq;
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

        [SerializeField] private Tilemap tilemap;
        [SerializeField] private MapCreator mapCreator;

        private TileType[] _tileTypes;
        private bool _show = true;

        [Inject] private InputEvents _inputEvents;

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
            tilemap.gameObject.SetActive(_show);
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

        public void Darken(bool dark)
        {
            tilemap.color = dark ? Color.grey : Color.white;
        }
    }
}