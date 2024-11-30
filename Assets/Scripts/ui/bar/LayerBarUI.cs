using System;
using System.Collections.Generic;
using influence;
using input;
using scriptableObjects.map;
using show;
using UnityEngine;
using Zenject;

namespace ui.bar
{
    public class LayerBarUI : MonoBehaviour
    {
        [SerializeField] private ButtonView prefab;

        private Dictionary<Layer?, ButtonView> _buttonViews = new();
        
        public List<LayerType> layerTypes;
        public Sprite emptyIcon;
        public ButtonView emptyButton;

        [Inject] private ShowStatusEvents _showStatusEvents;
        [Inject] private InputEvents _inputEvents;

        private void Awake()
        {
            CreateButtonViews();
        }

        private void OnEnable()
        {
            _showStatusEvents.OnShowLayer += LayerSelection;
        }

        private void OnDisable()
        {
            _showStatusEvents.OnShowLayer -= LayerSelection;
        }

        private void CreateButtonViews()
        {
            emptyButton = Instantiate(prefab, transform);
            emptyButton.SetData(emptyIcon, () => _inputEvents.ToggleShowInfluenceEvent(-1));
            emptyButton.SetSelected(true);
            
            layerTypes.ForEach(layer =>
            {
                var buttonView = Instantiate(prefab, transform);
                buttonView.SetData(layer.icon, () => _inputEvents.ToggleShowInfluenceEvent((int)layer.layer));
                _buttonViews.Add(layer.layer, buttonView);
                buttonView.SetSelected(false);
            });
        }

        private void LayerSelection(Layer? layer)
        {
            emptyButton.SetSelected(layer == null);
            
            foreach (var pair in _buttonViews)
            {
                pair.Value.SetSelected(pair.Key == layer);
            }
        }
    }
}