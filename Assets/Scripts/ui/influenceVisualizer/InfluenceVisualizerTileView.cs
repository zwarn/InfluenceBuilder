using TMPro;
using UnityEngine;

namespace ui.influenceVisualizer
{
    public class InfluenceVisualizerTileView : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void SetValue(double value)
        {
            string format = value switch
            {
                >= 100 => "F0",
                >= 10 => "F1",
                _ => "F"
            };

            text.text = value.ToString(format);
        }
    }
}