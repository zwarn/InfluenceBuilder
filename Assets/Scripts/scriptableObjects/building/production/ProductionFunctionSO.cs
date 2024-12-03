using System;
using System.Collections.Generic;
using System.Linq;
using influence;
using influence.buildings;
using UnityEngine;

namespace scriptableObjects.building.production
{
    [Serializable]
    [CreateAssetMenu(fileName = "BuildingType", menuName = "function/Production", order = 0)]
    public class ProductionFunctionSO : BuildingFunctionSO
    {
        [SerializeField] private int cooldown = 1;
        [SerializeField] private List<ProductionValue> production;


        public override BuildingFunction CreateFunction()
        {
            var layeredProduction =
                new Layered<double>(
                    production.Select(value => new KeyValuePair<Layer, double>(value.layer, value.value)),
                    Layered<double>.Addition());

            production.ForEach(entry =>
            {
                layeredProduction.AddOrUpdate(entry.layer, entry.value, Layered<double>.Addition());
            });

            return new ProductionFunction(layeredProduction, cooldown);
        }
    }

    [Serializable]
    public class ProductionValue
    {
        public Layer layer;
        public double value;
    }
}