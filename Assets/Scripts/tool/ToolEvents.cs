using System;
using scriptableObjects;

namespace tool
{
    public class ToolEvents
    {
        public event Action<SelectableTool> OnToolSelection;

        public void ToolSelectionEvent(SelectableTool tool)
        {
            OnToolSelection?.Invoke(tool);
        }

        public event Action<SelectableTool> OnToolSelected;

        public void ToolSelectedEvent(SelectableTool tool)
        {
            OnToolSelected?.Invoke(tool);
        }
    }
}