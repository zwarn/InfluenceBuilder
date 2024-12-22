using System;
using TMPro;
using UnityEngine;

namespace ui.buildingDetails
{
    public abstract class AspectRow<V> : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;

        public void Initialize(String labelText)
        {
            label.text = labelText;
        }

        public abstract void SetData(V values);
    }
}