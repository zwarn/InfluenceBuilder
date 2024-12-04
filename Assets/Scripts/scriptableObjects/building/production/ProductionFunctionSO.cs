using System;
using System.Collections.Generic;
using System.Linq;
using influence;
using influence.buildings;
using UnityEngine;
using UnityEngine.Serialization;

namespace scriptableObjects.building.production
{
    [Serializable]
    [CreateAssetMenu(fileName = "BuildingType", menuName = "function/Production", order = 0)]
    public class ProductionFunctionSO : BuildingFunctionSO
    {
        [SerializeField] private int cooldown = 1;
        [SerializeField] private List<ProductionInfo> production;
        [SerializeField] private List<ConsumptionInfo> consumption;


        public override BuildingFunction CreateFunction()
        {
            var layeredProduction = new Layered<double>(production.Select(value =>
                new KeyValuePair<Layer, double>(value.layer, value.production)), Layered<double>.Plus());

            var layeredConsumption = new Layered<ConsumptionData>(consumption.Select(info =>
                new KeyValuePair<Layer, ConsumptionData>(info.layer, new ConsumptionData(info.consumption,
                    info.localStorage, info.storageRate))), Layered<ConsumptionData>.Override());

            return new ProductionFunction(layeredProduction, layeredConsumption, cooldown);
        }
    }

    public class ConsumptionData
    {
        public readonly double Consumption;
        public readonly double LocalStorage;
        public readonly double StorageRate;

        public ConsumptionData(double consumption, double localStorage, double storageRate)
        {
            Consumption = consumption;
            LocalStorage = localStorage;
            StorageRate = storageRate;
        }
    }

    [Serializable]
    public class ProductionInfo
    {
        public Layer layer;
        public double production;
    }

    [Serializable]
    public class ConsumptionInfo
    {
        public Layer layer;
        public double consumption;
        public double localStorage;
        public double storageRate;
    }
}