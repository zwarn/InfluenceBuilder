using System;
using input;
using scriptableObjects;
using scriptableObjects.tool;
using UnityEngine;
using Zenject;

namespace tool
{
    public class ToolController : MonoBehaviour
    {
        private SelectableTool _currentTool;

        [Inject] private InputEvents _inputEvents;
        [Inject] private ToolEvents _toolEvents;

        private void OnEnable()
        {
            _inputEvents.OnLeftClick += LeftClick;
            _toolEvents.OnToolSelection += SetTool;
        }

        private void OnDisable()
        {
            _inputEvents.OnLeftClick -= LeftClick;
            _toolEvents.OnToolSelection -= SetTool;
        }

        private void SetTool(SelectableTool tool)
        {
            _currentTool = tool;
            _toolEvents.ToolSelectedEvent(tool);
        }

        private void LeftClick(Vector2Int pos)
        {
            if (_currentTool != null)
            {
                _currentTool.Apply(pos.x, pos.y);
            }
        }
    }
}