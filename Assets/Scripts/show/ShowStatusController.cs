using System;
using influence;
using input;
using lens;
using UnityEngine;
using Zenject;

namespace show
{
    public class ShowStatusController : MonoBehaviour
    {
        private bool _showTilemaps = true;
        private bool _showInfluenceVisualizer = false;

        private Lens _currentLens = null;
        private Lens _previewLens = null;

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
            _showEvents.ShowLensEvent(_currentLens);
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

        private void ToggleShowInfluence(Lens lens)
        {
            _currentLens = lens;
            _showEvents.ShowLensEvent(_currentLens);
            UpdateLayer();
        }

        private void PreviewInfluence(Lens lens)
        {
            _previewLens = lens;
            UpdateLayer();
        }

        private void UpdateLayer()
        {
            Lens shownLens = null;
            if (_previewLens != null)
            {
                shownLens = _previewLens;
            }
            else if (_currentLens != null)
            {
                shownLens = _currentLens;
            }

            _showEvents.ShowLensEvent(shownLens);
        }

        public Lens CurrentLens()
        {
            return _currentLens;
        }
    }
}