using System;
using input;
using UnityEngine;
using Zenject;

namespace show
{
    public class ShowStatusController : MonoBehaviour
    {
        private bool _showTilemaps = true;
        private bool _showInfluence = false;
        private bool _showInfluenceVisualizer = false;

        [Inject] private InputEvents _inputEvents;
        [Inject] private ShowStatusEvents _showEvents;

        private void OnEnable()
        {
            _inputEvents.OnToggleShowInfluence += ToggleShowInfluence;
            _inputEvents.OnToggleShowTilemap += ToggleShowTilemap;
            _inputEvents.OnToggleShowInfluenceVisualizer += ToggleShowInfluenceVisualizer;
        }

        private void OnDisable()
        {
            _inputEvents.OnToggleShowInfluence -= ToggleShowInfluence;
            _inputEvents.OnToggleShowTilemap -= ToggleShowTilemap;
            _inputEvents.OnToggleShowInfluenceVisualizer -= ToggleShowInfluenceVisualizer;
        }

        private void Start()
        {
            _showEvents.ShowTilemapEvent(_showTilemaps);
            _showEvents.ShowInfluenceEvent(_showInfluence);
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

        private void ToggleShowInfluence()
        {
            _showInfluence = !_showInfluence;
            _showEvents.ShowInfluenceEvent(_showInfluence);
        }
    }
}