using System.Collections.Generic;
using System.Linq;
using influence;
using scriptableObjects.map;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace map
{
    public class BuildingController : MonoBehaviour
    {
        private readonly Dictionary<Vector2Int, TileType> _buildings = new();

        [Inject] private InfluenceController _influenceController;
        [Inject] private MapController _mapController;

        public void ChangeTile(int x, int y, TileType tileType)
        {
            if (_buildings.ContainsKey(new Vector2Int(x, y)))
            {
                _buildings.Remove(new Vector2Int(x, y));
            }

            if (tileType.isBuilding)
            {
                _buildings.Add(new Vector2Int(x, y), tileType);
            }
        }

        public void Step()
        {
            _buildings.ToList().ForEach(pair =>
            {
                foreach (var transformation in pair.Value.Transformations)
                {
                    if (transformation.conditions.TrueForAll(condition =>
                            condition.Check(_influenceController, pair.Key)) && CanPay(transformation.costs, pair.Key))
                    {
                        Pay(transformation.costs, pair.Key);
                        _mapController.ChangeTile(pair.Key.x, pair.Key.y, transformation.successor);
                        return;
                    }
                }
            });
        }

        private void Pay(Dictionary<Layer, double> costs, Vector2Int position)
        {
            costs.Keys.ForEach(layer => _influenceController.RemoveStore(layer, position.x, position.y, costs[layer]));
        }

        private bool CanPay(Dictionary<Layer, double> costs, Vector2Int position)
        {
            return costs.Keys.All(layer =>
                _influenceController.GetStore(layer, position.x, position.y) >= costs[layer]);
        }
    }
}