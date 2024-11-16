using System;
using UnityEngine;

namespace input
{
    public class InputEvents
    {
        public event Action OnToggleAutomaticCommand;

        public void ToggleAutomaticEvent()
        {
            OnToggleAutomaticCommand?.Invoke();
        }

        public event Action OnPerformStepCommand;

        public void PerformStepCommanEvent()
        {
            OnPerformStepCommand?.Invoke();
        }

        public event Action<Vector2Int> OnAddInfluenceCommand;

        public void AddInfluenceCommandEvent(int x, int y)
        {
            OnAddInfluenceCommand?.Invoke(new Vector2Int(x, y));
        }

        public event Action<Vector2Int> OnRemoveInfluenceCommand;

        public void RemoveInfluenceCommandEvent(int x, int y)
        {
            OnRemoveInfluenceCommand?.Invoke(new Vector2Int(x, y));
        }

        public event Action OnZoomOut;

        public void ZoomOutEvent()
        {
            OnZoomOut?.Invoke();
        }

        public event Action OnZoomIn;

        public void ZoomInEvent()
        {
            OnZoomIn?.Invoke();
        }

        public event Action<Vector2> OnTranslate;

        public void TranslationEvent(Vector2 delta)
        {
            OnTranslate?.Invoke(delta);
        }
    }
}