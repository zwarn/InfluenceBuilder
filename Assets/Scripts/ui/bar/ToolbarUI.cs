using System.Collections.Generic;
using scriptableObjects.tool;
using tool;
using UnityEngine;
using Zenject;

namespace ui.bar
{
    public class ToolbarUI : MonoBehaviour
    {
        [SerializeField] private ButtonView prefab;
        public List<SelectableTool> toolSelection;

        private Dictionary<SelectableTool, ButtonView> _buttonViews = new();

        [Inject] private ToolEvents _toolEvents;

        private void Awake()
        {
            CreateButtonViews();
        }

        private void OnEnable()
        {
            _toolEvents.OnToolSelected += SelectTool;
        }

        private void OnDisable()
        {
            _toolEvents.OnToolSelected += SelectTool;
        }

        private void CreateButtonViews()
        {
            toolSelection.ForEach(tool =>
            {
                var buttonView = Instantiate(prefab, transform);
                buttonView.SetData(tool.icon, () => _toolEvents.ToolSelectionEvent(tool));
                _buttonViews.Add(tool, buttonView);
                buttonView.SetSelected(false);
            });
        }

        private void SelectTool(SelectableTool tool)
        {
            foreach (var pair in _buttonViews)
            {
                pair.Value.SetSelected(pair.Key == tool);
            }
        }
    }
}