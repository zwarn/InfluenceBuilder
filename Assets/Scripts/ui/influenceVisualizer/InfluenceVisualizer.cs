using System;
using System.Collections.Generic;
using influence;
using map;
using TMPro;
using UnityEngine;
using Zenject;

namespace ui.influenceVisualizer
{
    public class InfluenceVisualizer : MonoBehaviour
    {
        private const int GridSize = 3;

        [SerializeField] private InfluenceVisualizerTileView prefab;

        private Dictionary<Vector2Int, InfluenceVisualizerTileView> _tileViews = new();

        [Inject] private MapController _mapController;
        [Inject] private InfluenceController _influenceController;

        private void Start()
        {
            for (int x = -GridSize + 1; x < GridSize; x++)
            {
                for (int y = -GridSize + 1; y < GridSize; y++)
                {
                    Vector2Int relativPos = new Vector2Int(x, y);
                    var tileView = Instantiate(prefab, transform);
                    tileView.transform.localPosition = new Vector3(x, y);
                    _tileViews.Add(relativPos, tileView);
                }
            }
        }

        private void Update()
        {
            var worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var x = (int)worldMouse.x;
            var y = (int)worldMouse.y;

            if (_mapController.IsPointOnMap(x, y))
            {
                transform.position = new Vector3(x, y);
            }

            UpdateTileViews(x, y);
        }

        private void UpdateTileViews(int centerX, int centerY)
        {
            for (int x = -GridSize + 1; x < GridSize; x++)
            {
                for (int y = -GridSize + 1; y < GridSize; y++)
                {
                    int worldX = centerX + x;
                    int worldY = centerY + y;
                    var tileView = _tileViews[new Vector2Int(x, y)];

                    if (_mapController.IsPointOnMap(worldX, worldY))
                    {
                        tileView.SetValue(_influenceController.GetGrid().GetValue(worldX, worldY));
                        tileView.gameObject.SetActive(true);
                    }
                    else
                    {
                        tileView.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}