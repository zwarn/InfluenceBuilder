using System;
using influence;
using UnityEngine;
using Zenject;

namespace input
{
    public class InputController : MonoBehaviour
    {
        [Inject] private GameController _gameController;
        [Inject] private InfluenceController _influenceController;

        private void Update()
        {
            if (Input.GetButtonUp("Jump"))
            {
                _gameController.Tick();
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                _gameController.ToggleAutomatic();
            }

            if (Input.GetMouseButton(0))
            {
                var worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var x = (int)worldMouse.x;
                var y = (int)worldMouse.y;
                _influenceController.AddInfluence(x, y);
            }

            if (Input.GetMouseButton(1))
            {
                var worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var x = (int)worldMouse.x;
                var y = (int)worldMouse.y;
                _influenceController.RemoveInfluence(x, y);
            }
        }
    }
}