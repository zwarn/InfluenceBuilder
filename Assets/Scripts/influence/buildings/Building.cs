using UnityEngine;

namespace influence.buildings
{
    public class Building
    {
        public readonly Vector2Int Position;
        public readonly BuildingType BuildingType;
        private InfluenceController _influenceController;

        public Building(BuildingType buildingType, Vector2Int position, InfluenceController influenceController)
        {
            BuildingType = buildingType;
            Position = position;
            _influenceController = influenceController;
        }

        public void Init()
        {
            BuildingType.Functions.ForEach(function => function.Init(Position, _influenceController));
        }

        public void Remove()
        {
            BuildingType.Functions.ForEach(function => function.Remove());
        }

        public void Step()
        {
            BuildingType.Functions.ForEach(function => function.Step());
        }
    }
}