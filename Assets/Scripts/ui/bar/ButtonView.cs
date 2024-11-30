using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ui.bar
{
    public class ButtonView : MonoBehaviour, IPointerClickHandler
    {
        public Image iconImage;
        public Image image;
        public Sprite button;
        public Sprite activeButton;

        private Action _onClick;

        public void SetData(Sprite icon, Action onClick)
        {
            iconImage.sprite = icon;
            _onClick = onClick;
        }

        public void SetSelected(bool highlight)
        {
            image.sprite = highlight ? activeButton : button;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick.Invoke();
        }
    }
}