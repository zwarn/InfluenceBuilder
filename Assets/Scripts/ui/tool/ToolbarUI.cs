using System;
using System.Collections.Generic;
using System.Linq;
using scriptableObjects;
using UnityEngine;

namespace ui
{
    public class ToolbarUI : MonoBehaviour
    {
        [SerializeField] private ToolView prefab;
        [SerializeField] private List<SelectableTool> toolSelection;

        private Dictionary<SelectableTool, ToolView> _toolViews = new();

        private void Awake()
        {
            CreateToolViews();
        }

        private void CreateToolViews()
        {
            toolSelection.ForEach(scriptableObject =>
            {
                var toolView = Instantiate(prefab, transform);
                toolView.SetTool(scriptableObject);
                _toolViews.Add(scriptableObject, toolView);
                toolView.Highlight(false);
            });

            _toolViews.Values.First().Highlight(true);
        }
    }
}