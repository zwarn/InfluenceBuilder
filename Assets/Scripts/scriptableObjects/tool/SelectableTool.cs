using UnityEngine;

namespace scriptableObjects.tool
{
    public abstract class SelectableTool : ScriptableObject
    {
        public Sprite icon;
        public string name;

        public abstract void Apply(int x, int y);
    }
}