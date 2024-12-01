using System;
using influence;
using UnityEngine;

namespace scriptableObjects.building
{
    [Serializable]
    [CreateAssetMenu(fileName = "BuildingType", menuName = "function/Production", order = 0)]
    public class ProductionFunctionSO : BuildingFunctionSO
    {
        [SerializeField] private double production;
        [SerializeField] private int cooldown = 1;
        [SerializeField] private Layer layer;


        public override BuildingFunction CreateFunction()
        {
            return new ProductionFunction(production, cooldown, layer);
        }
    }
}