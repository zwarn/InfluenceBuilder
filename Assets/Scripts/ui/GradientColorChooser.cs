using System;
using UnityEngine;

namespace ui
{
    public class GradientColorChooser : MonoBehaviour, IColorChooser
    {
        [SerializeField] private double minValue = 0.001;
        [SerializeField] private double maxValue = 1000;
        [SerializeField] private Gradient colorGradient;


        public Color GetColor(double value)
        {
            float t = (Mathf.Log10((float)value) - Mathf.Log10((float)minValue)) /
                      (Mathf.Log10((float)maxValue) - Mathf.Log10((float)minValue));

            return colorGradient.Evaluate(t);
        }
    }
}