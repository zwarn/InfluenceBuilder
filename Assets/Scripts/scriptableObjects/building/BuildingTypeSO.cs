using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace scriptableObjects.building
{
    [Serializable]
    [CreateAssetMenu(fileName = "BuildingType", menuName = "building/BuildingType", order = 0)]
    public class BuildingTypeSO : ScriptableObject
    {
        [SerializeField] public List<BuildingFunctionSO> functions;
        
        public BuildingType CreateBuildingType()
        {
            return new BuildingType(functions.Select(so => so.CreateFunction()).ToList());
        }
    }
}