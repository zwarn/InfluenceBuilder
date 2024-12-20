using lens;
using UnityEngine;

namespace scriptableObjects.lens
{
    [CreateAssetMenu(fileName = "HappinessLensType", menuName = "lens/HappinessLens", order = 0)]
    public class HappinessLensType : LensType
    {
        public Sprite icon;

        public override Sprite GetIcon()
        {
            return icon;
        }

        public override Lens GetLens()
        {
            return new HappinessLens();
        }
    }
}