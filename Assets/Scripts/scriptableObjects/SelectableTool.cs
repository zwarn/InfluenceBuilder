using System;
using UnityEngine;

namespace scriptableObjects
{
    public abstract class SelectableTool : ScriptableObject
    {
        public Sprite icon;
        public String name;

        public abstract void Apply(int x, int y);
    }
}