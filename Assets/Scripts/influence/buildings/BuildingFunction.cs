using influence;
using UnityEngine;

namespace scriptableObjects.building
{
    public abstract class BuildingFunction
    {
        public abstract void Init(Vector2Int position, InfluenceController influenceController);

        public abstract void Step();

        public abstract void Remove();
    }
}