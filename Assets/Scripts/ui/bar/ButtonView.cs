using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ui.bar
{
    public class ButtonView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Image iconImage;
        public Image image;
        public Sprite button;
        public Sprite activeButton;

        private Action _onClick;
        private Action _onHoverEnter;
        private Action _onHoverLeave;

        public void SetData(Sprite icon, Action onClick, Action onHoverEnter, Action onHoverLeave)
        {
            iconImage.sprite = icon;
            _onClick = onClick;
            _onHoverEnter = onHoverEnter;
            _onHoverLeave = onHoverLeave;
        }

        public void SetSelected(bool highlight)
        {
            image.sprite = highlight ? activeButton : button;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _onHoverEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _onHoverLeave?.Invoke();
        }
    }
}