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

        public event Action<Vector2Int> OnLeftClick;

        public void LeftClickEvent(int x, int y)
        {
            OnLeftClick?.Invoke(new Vector2Int(x, y));
        }

        public event Action<Vector2Int> OnRightClick;

        public void RightClickEvent(int x, int y)
        {
            OnRightClick?.Invoke(new Vector2Int(x, y));
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

        public event Action OnToggleShowInfluence;
        
        public void ToggleShowInfluenceEvent()
        {
            OnToggleShowInfluence?.Invoke();
        }

        public event Action OnToggleShowTilemap;
        
        public void ToggleShowTilemapEvent()
        {
            OnToggleShowTilemap?.Invoke();
        }
    }
}