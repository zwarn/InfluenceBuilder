using System;
using influence;
using UnityEngine;

namespace scriptableObjects.map
{
    [CreateAssetMenu(fileName = "LayerType", menuName = "map/LayerType", order = 0)]
    public class LayerType : ScriptableObject
    {
        public Sprite icon;
        public Layer layer;
    }
}