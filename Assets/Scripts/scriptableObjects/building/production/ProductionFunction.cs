using System;
using System.Linq;
using influence;
using influence.buildings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace scriptableObjects.building.production
{
    public class ProductionFunction : BuildingFunction
    {
        private readonly Layered<double> _production;
        private readonly Layered<ConsumptionData> _consumption;
        private readonly Layered<double> _storage;
        private readonly int _cooldown;

        private Vector2Int _position;
        private InfluenceController _influenceController;
        private int _stepCounter;

        public ProductionFunction(Layered<double> production, Layered<ConsumptionData> consumption, int cooldown)
        {
            _production = production;
            _consumption = consumption;
            _cooldown = cooldown;
            _storage = _consumption.Select(_ => 0d);
        }

        public override void Init(Vector2Int position, InfluenceController influenceController)
        {
            _position = position;
            _influenceController = influenceController;
            _stepCounter = Random.Range(0, _cooldown - 1);
        }

        public override void Step()
        {
            _stepCounter++;

            Consume();

            if (_stepCounter >= _cooldown)
            {
                _stepCounter = 0;
                Produce();
            }
        }

        private void Consume()
        {
            _consumption.ForEach((layer, value) =>
            {
                double storageRate = Math.Min(value.StorageRate, value.LocalStorage - _storage.Get(layer));
                double store = _influenceController.RemoveInfluence(layer, _position.x, _position.y, storageRate);
                _storage.AddOrUpdate(layer, store, Layered<double>.Plus());
            });
        }

        private void Produce()
        {
            var consumptionPossible = _storage.ToDictionary().All((pair) =>
            {
                return pair.Value >= _consumption.Get(pair.Key).Consumption;
            });

            if (consumptionPossible)
            {
                _production.ForEach((layer, value) =>
                {
                    _influenceController.AddInfluence(layer, _position.x, _position.y, value);
                });
                _consumption.ForEach((layer, data) =>
                    _storage.AddOrUpdate(layer, data.Consumption, Layered<double>.MinusMin0()));
            }
        }

        public override void Remove()
        {
            _storage.ForEach(
                (layer, value) => _influenceController.AddInfluence(layer, _position.x, _position.y, value));
        }
    }
}