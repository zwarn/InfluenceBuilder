using System.Collections.Generic;
using scriptableObjects;
using scriptableObjects.tool;
using tool;
using UnityEngine;
using Zenject;

namespace ui.tool
{
    public class ToolbarUI : MonoBehaviour
    {
        [SerializeField] private ToolView prefab;
        public List<SelectableTool> toolSelection;

        private Dictionary<SelectableTool, ToolView> _toolViews = new();

        [Inject] private DiContainer _diContainer;
        [Inject] private ToolEvents _toolEvents;

        private void Awake()
        {
            CreateToolViews();
        }

        private void OnEnable()
        {
            _toolEvents.OnToolSelected += SelectTool;
        }

        private void OnDisable()
        {
            _toolEvents.OnToolSelected += SelectTool;
        }

        private void CreateToolViews()
        {
            toolSelection.ForEach(scriptableObject =>
            {
                var childObject = _diContainer.InstantiatePrefab(prefab, transform);
                var toolView = childObject.GetComponent<ToolView>();
                toolView.SetTool(scriptableObject);
                _toolViews.Add(scriptableObject, toolView);
                toolView.Highlight(false);
            });
        }

        private void SelectTool(SelectableTool tool)
        {
            foreach (var pair in _toolViews)
            {
                pair.Value.Highlight(pair.Key == tool);
            }
        }
    }
}