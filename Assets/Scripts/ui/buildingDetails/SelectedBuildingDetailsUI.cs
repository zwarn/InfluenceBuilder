using System;
using System.Collections.Generic;
using System.Linq;
using helper;
using influence;
using map;
using show;
using TMPro;
using UnityEngine;
using Zenject;

namespace ui.buildingDetails
{
    public class SelectedBuildingDetailsUI : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private StoreAspectPanel storePanel;
        [SerializeField] private ConsumptionAspectPanel consumptionPanel;

        private Vector2Int? _currentSelection;

        [Inject] private ShowStatusEvents _showStatusEvents;
        [Inject] private InfluenceController _influenceController;
        [Inject] private MapController _mapController;

        private void OnEnable()
        {
            _showStatusEvents.OnSelectTile += OnSelectTile;
        }

        private void OnDisable()
        {
            _showStatusEvents.OnSelectTile -= OnSelectTile;
        }

        private void Start()
        {
            Dictionary<Layer, String> layerLabels = new Dictionary<Layer, string>();
            int layers = EnumUtils.GetLength<Layer>();
            for (int i = 0; i < layers; i++)
            {
                Layer layer = (Layer)i;
                layerLabels.Add(layer, layer.ToString());
            }

            storePanel.Initialize("store", layerLabels);
            consumptionPanel.Initialize("consumption", layerLabels);
        }

        private void Update()
        {
            content.gameObject.SetActive(_currentSelection.HasValue);
            ShowBuildingDetails();
        }

        private void ShowBuildingDetails()
        {
            if (!_currentSelection.HasValue)
            {
                return;
            }

            var position = _currentSelection.Value;
            var tileType = _mapController.GetTileType(position.x, position.y);

            nameLabel.text = tileType.name;

            storePanel.SetData(tileType.Store.ToDictionary(pair => pair.Key,
                pair => new CurrentMax(_influenceController.GetStore(pair.Key, position.x, position.y),
                    pair.Value.storeSize)));

            consumptionPanel.SetData(tileType.Consumption.ToDictionary(pair => pair.Key,
                pair => new ConsumptionData(
                    _influenceController.GetStore(pair.Key, position.x, position.y),
                    pair.Value.consumption,
                    _influenceController.GetHappiness(position.x, position.y),
                    pair.Value.weight)));
        }

        private void OnSelectTile(Vector2Int position)
        {
            _currentSelection = position;
        }
    }
}