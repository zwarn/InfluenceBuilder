using System;
using System.Collections.Generic;
using influence;
using lens;
using map;
using show;
using UnityEngine;
using Zenject;

namespace ui.influenceVisualizer
{
    public class InfluenceVisualizer : MonoBehaviour
    {
        private const int GridSize = 3;

        [SerializeField] private InfluenceVisualizerTileView prefab;
        [SerializeField] private Transform tileParent;

        private Dictionary<Vector2Int, InfluenceVisualizerTileView> _tileViews = new();
        private bool _show;

        [Inject] private MapController _mapController;
        [Inject] private InfluenceController _influenceController;
        [Inject] private ShowStatusEvents _showStatusEvents;
        [Inject] private ShowStatusController _showStatusController;

        private void Start()
        {
            for (int x = -GridSize + 1; x < GridSize; x++)
            {
                for (int y = -GridSize + 1; y < GridSize; y++)
                {
                    Vector2Int relativPos = new Vector2Int(x, y);
                    var tileView = Instantiate(prefab, tileParent);
                    tileView.transform.localPosition = new Vector3(x, y);
                    _tileViews.Add(relativPos, tileView);
                }
            }
        }

        private void OnEnable()
        {
            _showStatusEvents.OnShowInfluenceVisualizer += OnShowVisualizer;
        }

        private void OnDisable()
        {
            _showStatusEvents.OnShowInfluenceVisualizer -= OnShowVisualizer;
        }

        private void OnShowVisualizer(bool show)
        {
            _show = show;
            tileParent.gameObject.SetActive(_show);
        }

        private void Update()
        {
            if (!_show)
            {
                return;
            }

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
            Lens currentLens = _showStatusController.CurrentLens();
            if (currentLens == null)
            {
                return;
            }

            for (int x = -GridSize + 1; x < GridSize; x++)
            {
                for (int y = -GridSize + 1; y < GridSize; y++)
                {
                    int worldX = centerX + x;
                    int worldY = centerY + y;
                    var tileView = _tileViews[new Vector2Int(x, y)];

                    if (_mapController.IsPointOnMap(worldX, worldY))
                    {
                        tileView.SetValue(currentLens.GetValue(_influenceController, worldX, worldY));
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