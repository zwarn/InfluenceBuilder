using TMPro;
using UnityEngine;

namespace ui.influenceVisualizer
{
    public class InfluenceVisualizerTileView : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void SetValue(double value)
        {
            text.text = value.ToString("F");
        }
    }
}