using System.Collections.Generic;
using influence;
using Sirenix.OdinInspector;
using UnityEngine;

namespace scriptableObjects.map
{
    [CreateAssetMenu(fileName = "Properties", menuName = "map/Properties", order = 0)]
    public class TileProperties : SerializedScriptableObject
    {
        public readonly Dictionary<Layer, Properties> Properties = new();
    }
}