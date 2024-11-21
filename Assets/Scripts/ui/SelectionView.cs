using System;
using map;
using UnityEngine;
using Zenject;

namespace ui
{
    public class SelectionView : MonoBehaviour
    {
        [SerializeField] private GameObject Image;

        [Inject] private MapController _mapController;

        private void Update()
        {
            var worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var x = (int)worldMouse.x;
            var y = (int)worldMouse.y;

            if (_mapController.IsPointOnMap(x, y))
            {
                transform.position = new Vector3(x, y);
            }

            Image.SetActive(_mapController.IsPointOnMap(x, y));
        }
    }
}