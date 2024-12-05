using System;
using influence;
using input;
using UnityEngine;
using Zenject;

namespace show
{
    public class ShowStatusController : MonoBehaviour
    {
        private bool _showTilemaps = true;
        private bool _showInfluenceVisualizer = false;

        private Layer? _currentLayer = null;
        private Layer? _previewLayer = null;

        [Inject] private InputEvents _inputEvents;
        [Inject] private ShowStatusEvents _showEvents;

        private void OnEnable()
        {
            _inputEvents.OnToggleShowInfluence += ToggleShowInfluence;
            _inputEvents.OnPreviewInfluence += PreviewInfluence;
            _inputEvents.OnToggleShowTilemap += ToggleShowTilemap;
            _inputEvents.OnToggleShowInfluenceVisualizer += ToggleShowInfluenceVisualizer;
        }
        
        private void OnDisable()
        {
            _inputEvents.OnToggleShowInfluence -= ToggleShowInfluence;
            _inputEvents.OnPreviewInfluence += PreviewInfluence;
            _inputEvents.OnToggleShowTilemap -= ToggleShowTilemap;
            _inputEvents.OnToggleShowInfluenceVisualizer -= ToggleShowInfluenceVisualizer;
        }

        private void Start()
        {
            _showEvents.ShowTilemapEvent(_showTilemaps);
            _showEvents.ShowLayerEvent(_currentLayer);
            _showEvents.ShowInfluenceVisualizerEvent(_showInfluenceVisualizer);
        }

        private void ToggleShowInfluenceVisualizer()
        {
            _showInfluenceVisualizer = !_showInfluenceVisualizer;
            _showEvents.ShowInfluenceVisualizerEvent(_showInfluenceVisualizer);
        }

        private void ToggleShowTilemap()
        {
            _showTilemaps = !_showTilemaps;
            _showEvents.ShowTilemapEvent(_showTilemaps);
        }

        private void ToggleShowInfluence(int layer)
        {
            var values = Enum.GetValues(typeof(Layer));
            _currentLayer = layer == -1 || layer >= values.Length ? null : (Layer)layer;
            _showEvents.ShowLayerEvent(_currentLayer);
            UpdateLayer();
        }
        
        private void PreviewInfluence(int layer)
        {
            var values = Enum.GetValues(typeof(Layer));
            _previewLayer = layer == -1 || layer >= values.Length ? null : (Layer)layer;
            UpdateLayer();
        }

        private void UpdateLayer()
        {
            Layer? shownLayer = null;
            if (_previewLayer.HasValue)
            {
                shownLayer = _previewLayer.Value;
            }
            else if (_currentLayer.HasValue)
            {
                shownLayer = _currentLayer.Value;
            }
            _showEvents.ShowLayerEvent(shownLayer);
        }

        public Layer? CurrentLayer()
        {
            return _currentLayer;
        }
    }
}