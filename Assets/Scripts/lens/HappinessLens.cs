using influence;

namespace lens
{
    public class HappinessLens : Lens
    {
        public override double[] GetValues(InfluenceController influenceController)
        {
            return influenceController.GetHappiness();
        }

        public override double GetValue(InfluenceController influenceController, int x, int y)
        {
            return influenceController.GetHappiness(x, y);
        }

        public override float MinValue => 0f;

        public override float MaxValue => 1f;

        public override float Alpha => 0.8f;

        public override bool IsExponential => false;

        private bool Equals(HappinessLens other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HappinessLens)obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}