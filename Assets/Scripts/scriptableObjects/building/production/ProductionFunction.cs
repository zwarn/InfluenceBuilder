using influence;
using influence.buildings;
using UnityEngine;

namespace scriptableObjects.building.production
{
    public class ProductionFunction : BuildingFunction
    {
        private readonly Layered<double> _production;
        private readonly int _cooldown;

        private Vector2Int _position;
        private InfluenceController _influenceController;
        private int _stepCounter;

        public ProductionFunction(Layered<double> production, int cooldown)
        {
            _production = production;
            _cooldown = cooldown;
        }

        public override void Init(Vector2Int position, InfluenceController influenceController)
        {
            _position = position;
            _influenceController = influenceController;
            _stepCounter = 0;
        }

        public override void Step()
        {
            _stepCounter++;
            if (_stepCounter >= _cooldown)
            {
                _stepCounter = 0;
                Produce();
            }
        }

        private void Produce()
        {
            _production.ForEach((layer, value) =>
                _influenceController.AddInfluence(layer, _position.x, _position.y, value));
        }

        public override void Remove()
        {
        }
    }
}