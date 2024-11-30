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
        public Image image;
        public Sprite button;
        public Sprite activeButton;

        private SelectableTool _tool;

        [Inject] private ToolEvents _toolEvents;

        public void SetTool(SelectableTool tool)
        {
            _tool = tool;
            icon.sprite = tool.icon;
        }

        public void SetSelected(bool highlight)
        {
            image.sprite = highlight ? activeButton : button;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _toolEvents.ToolSelectionEvent(_tool);
        }
    }
}