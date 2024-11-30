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
        }

        public void Remove()
        {
        }

        public void Step()
        {
            _influenceController.AddInfluence(_buildingType.layer, _position.x, _position.y, _buildingType.production);
        }
    }
}