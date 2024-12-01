using System;
using System.Collections.Generic;
using scriptableObjects.building;
using UnityEngine;
using Zenject;

namespace influence.buildings
{
    public class BuildingController : MonoBehaviour
    {
        private Dictionary<Vector2Int, Building> _buildings = new();

        [Inject] private InfluenceController _influenceController;

        public void AddBuilding(Vector2Int position, BuildingType buildingType)
        {
            if (_buildings.TryGetValue(position, out var old))
            {
                if (buildingType.BuildingTypeSO == old.BuildingType.BuildingTypeSO)
                {
                    return;
                }

                RemoveBuilding(position);
            }

            var building = new Building(buildingType, position, _influenceController);
            _buildings[position] = building;
            building.Init();
        }

        public void RemoveBuilding(Vector2Int position)
        {
            if (_buildings.ContainsKey(position))
            {
                _buildings[position].Remove();
                _buildings.Remove(position);
            }
        }

        public void Step()
        {
            foreach (var building in _buildings.Values)
            {
                building.Step();
            }
        }
    }
}