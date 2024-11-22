using System;
using influence;
using map;
using show;
using UnityEngine;
using Zenject;

namespace ui
{
    public class InfluenceGridUI : MonoBehaviour
    {
        [SerializeField] private Renderer quad;
        private Texture2D _texture2D;
        private Layer? _currentLayer = Layer.Food;

        [Inject] private InfluenceController _influenceController;
        [Inject] private IColorChooser _colorChooser;
        [Inject] private GridEvents _gridEvents;
        [Inject] private ShowStatusEvents _showStatusEvents;
        [Inject] private MapController _mapController;

        private void Start()
        {
            int width = _mapController.width;
            int height = _mapController.height;

            quad.transform.position = new Vector3(width / 2f, height / 2f, -1);
            quad.transform.localScale = new Vector3(width, height, width);

            _texture2D = new Texture2D(width, height);
            _texture2D.filterMode = FilterMode.Point;
            OnUpdate();
            quad.material.mainTexture = _texture2D;
        }

        private void OnEnable()
        {
            _gridEvents.OnGridUpdate += OnUpdate;
            _showStatusEvents.OnShowLayer += OnShowLayer;
        }

        private void OnDisable()
        {
            _gridEvents.OnGridUpdate -= OnUpdate;
            _showStatusEvents.OnShowLayer -= OnShowLayer;
        }

        private void OnShowLayer(Layer? layer)
        {
            _currentLayer = layer;
            quad.gameObject.SetActive(layer != null);
            _mapController.Darken(layer != null);
            if (layer != null)
            {
                UpdateLayer(layer.Value);
            }
        }

        private void OnUpdate()
        {
            if (_currentLayer == null)
            {
                return;
            }

            UpdateLayer(_currentLayer.Value);
        }

        private void UpdateLayer(Layer layer)
        {
            int index = 0;
            var values = _influenceController.GetValues(layer);
            var colors = new Color[values.Length];

            while (index < values.Length)
            {
                colors[index] = _colorChooser.GetColor(values[index]);
                index++;
            }

            _texture2D.SetPixels(colors);
            _texture2D.Apply();
        }
    }
}