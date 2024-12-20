using lens;
using UnityEngine;

namespace scriptableObjects.lens
{
    public abstract class LensType : ScriptableObject
    {
        public abstract Sprite GetIcon();
        public abstract Lens GetLens();
    }
}