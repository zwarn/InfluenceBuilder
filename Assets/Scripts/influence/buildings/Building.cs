using scriptableObjects.building;
using UnityEngine;

namespace influence.buildings
{
    public class Building
    {
        private Vector2Int _position;
        private InfluenceController _influenceController;
        private BuildingType _buildingType;

        public Building(BuildingType buildingType, Vector2Int position, InfluenceController influenceController)
        {
            _buildingType = buildingType;
            _position = position;
            _influenceController = influenceController;
        }

        public void Init()
        {
            _buildingType.Functions.ForEach(function => function.Init(_position, _influenceController));
        }

        public void Remove()
        {
            _buildingType.Functions.ForEach(function => function.Remove());
        }

        public void Step()
        {
            _buildingType.Functions.ForEach(function => function.Step());
        }
    }
}