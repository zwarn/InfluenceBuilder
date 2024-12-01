using System.Collections.Generic;
using System.Linq;
using scriptableObjects.building;

namespace influence.buildings
{
    public class BuildingType
    {
        public readonly List<BuildingFunction> Functions;
        public readonly BuildingTypeSO BuildingTypeSO;

        public BuildingType(BuildingTypeSO buildingTypeSO)
        {
            BuildingTypeSO = buildingTypeSO;
            Functions = buildingTypeSO.functions.Select(so => so.CreateFunction()).ToList();
        }
    }
}