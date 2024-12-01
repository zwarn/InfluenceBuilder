using System.Collections.Generic;

namespace scriptableObjects.building
{
    public class BuildingType
    {
        public List<BuildingFunction> Functions;

        public BuildingType(List<BuildingFunction> functions)
        {
            Functions = functions;
        }
    }
}