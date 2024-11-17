using scriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ui
{
    public class ToolView : MonoBehaviour
    {
        [FormerlySerializedAs("image")] public Image icon;
        public Image border;
        public Color highlightColor;
        public Color lowLightColor;

        public void SetTool(SelectableTool tool)
        {
            icon.sprite = tool.icon;
        }

        public void Highlight(bool highlight)
        {
            border.color = highlight ? highlightColor : lowLightColor;
        }
    }
}