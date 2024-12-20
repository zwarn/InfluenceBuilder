using influence;
using lens;
using UnityEngine;

namespace scriptableObjects.lens
{
    [CreateAssetMenu(fileName = "LayerLensType", menuName = "lens/LayerLensType", order = 0)]
    public class LayerLensType : LensType
    {
        public Sprite icon;
        public Layer layer;

        public override Sprite GetIcon()
        {
            return icon;
        }

        public override Lens GetLens()
        {
            return new LayerLens(layer);
        }
    }
}