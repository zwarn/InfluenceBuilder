using influence;

namespace lens
{
    public class LayerLens : Lens
    {

        private readonly Layer _layer;

        public LayerLens(Layer layer)
        {
            _layer = layer;
        }

        public Layer GetLayer()
        {
            return _layer;
        }
        
        public override double[] GetValues(InfluenceController influenceController)
        {
            return influenceController.GetValues(_layer);
        }
        public override double GetValue(InfluenceController influenceController, int x, int y)
        {
            return influenceController.GetValue(_layer, x, y);
        }

        public override float MinValue => 0.001f;

        public override float MaxValue => 5000;

        public override float Alpha => 0.8f;

        public override bool IsExponential => true;

        private bool Equals(LayerLens other)
        {
            return _layer == other._layer;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LayerLens)obj);
        }

        public override int GetHashCode()
        {
            return (int)_layer;
        }
    }
}
