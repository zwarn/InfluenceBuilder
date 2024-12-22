using System;
using TMPro;
using UnityEngine;

namespace ui.buildingDetails
{
    public class AspectRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private TMP_Text current;
        [SerializeField] private TMP_Text max;

        public void Initialize(String labelText)
        {
            label.text = labelText;
        }

        public void SetData((double, double) values)
        {
            current.text = values.Item1.ToString("F1");
            max.text = values.Item2.ToString("F1");
        }
    }
}