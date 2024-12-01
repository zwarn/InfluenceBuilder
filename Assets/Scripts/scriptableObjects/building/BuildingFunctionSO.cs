using System;
using influence.buildings;
using UnityEngine;

namespace scriptableObjects.building
{
    [Serializable]
    public abstract class BuildingFunctionSO : ScriptableObject
    {
        public abstract BuildingFunction CreateFunction();
    }
}