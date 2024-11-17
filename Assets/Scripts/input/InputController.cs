﻿using UnityEngine;
using Zenject;

namespace input
{
    public class InputController : MonoBehaviour
    {
        [Inject] private InputEvents _inputEvents;

        private void Update()
        {
            if (Input.GetButtonUp("Jump"))
            {
                _inputEvents.PerformStepCommanEvent();
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                _inputEvents.ToggleAutomaticEvent();
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                _inputEvents.ToggleShowInfluenceEvent();
            }

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                _inputEvents.ToggleShowTilemapEvent();
            }

            if (Input.GetMouseButton(0))
            {
                var worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var x = (int)worldMouse.x;
                var y = (int)worldMouse.y;
                _inputEvents.LeftClickEvent(x, y);
            }

            if (Input.GetMouseButton(1))
            {
                var worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var x = (int)worldMouse.x;
                var y = (int)worldMouse.y;
                _inputEvents.RightClickEvent(x, y);
            }

            HandleTranslate();
            HandleZoom();
        }

        private void HandleTranslate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var delta = new Vector2(horizontal, vertical);
            if (delta.magnitude > 0.1)
            {
                _inputEvents.TranslationEvent(delta);
            }
        }

        private void HandleZoom()
        {
            var mouseScroll = Input.mouseScrollDelta.y;

            if (mouseScroll > 0.5f)
            {
                _inputEvents.ZoomInEvent();
            }
            else if (mouseScroll < -0.5f)
            {
                _inputEvents.ZoomOutEvent();
            }
        }
    }
}