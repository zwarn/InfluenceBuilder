using System;
using influence;

namespace lens
{
    public abstract class Lens
    {
        public abstract double[] GetValues(InfluenceController influenceController);

        public abstract float MinValue { get; }

        public abstract float MaxValue { get; }

        public abstract float Alpha { get; }

        public abstract bool IsExponential { get; }

        public abstract double GetValue(InfluenceController influenceController, int x, int y);
    }
}