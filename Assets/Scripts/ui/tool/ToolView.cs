using scriptableObjects;
using scriptableObjects.tool;
using tool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace ui.tool
{
    public class ToolView : MonoBehaviour, IPointerClickHandler
    {
        public Image icon;
        public Image border;
        public Color highlightColor;
        public Color lowLightColor;

        private SelectableTool _tool;

        [Inject] private ToolEvents _toolEvents;

        public void SetTool(SelectableTool tool)
        {
            _tool = tool;
            icon.sprite = tool.icon;
        }

        public void Highlight(bool highlight)
        {
            border.color = highlight ? highlightColor : lowLightColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _toolEvents.ToolSelectionEvent(_tool);
        }
    }
}