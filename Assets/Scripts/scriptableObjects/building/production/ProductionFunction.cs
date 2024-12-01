using influence;
using influence.buildings;
using UnityEngine;

namespace scriptableObjects.building
{
    public class ProductionFunction : BuildingFunction
    {
        private readonly double _production;
        private readonly int _cooldown;
        private readonly Layer _layer;

        private Vector2Int _position;
        private InfluenceController _influenceController;
        private int _stepCounter;

        public ProductionFunction(double production, int cooldown, Layer layer)
        {
            _production = production;
            _cooldown = cooldown;
            _layer = layer;
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
            _influenceController.AddInfluence(_layer, _position.x, _position.y, _production);
        }

        public override void Remove()
        {
        }
    }
}