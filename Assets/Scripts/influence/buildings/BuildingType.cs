using System;

namespace influence.buildings
{
    [Serializable]
    public class BuildingType
    {
        public double production;
        public Layer layer;

        public BuildingType(double production, Layer layer)
        {
            this.production = production;
            this.layer = layer;
        }
    }
}