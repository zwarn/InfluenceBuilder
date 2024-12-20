﻿using System;
using lens;
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

        public event Action<Vector2Int> OnUseTool;

        public void UseToolEvent(int x, int y)
        {
            OnUseTool?.Invoke(new Vector2Int(x, y));
        }

        public event Action<Vector2Int> OnAltUseTool;

        public void AltUseToolEvent(int x, int y)
        {
            OnAltUseTool?.Invoke(new Vector2Int(x, y));
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

        public event Action<Lens> OnToggleShowInfluence;

        public void ToggleShowInfluenceEvent(Lens lens)
        {
            OnToggleShowInfluence?.Invoke(lens);
        }

        public event Action OnToggleShowTilemap;

        public void ToggleShowTilemapEvent()
        {
            OnToggleShowTilemap?.Invoke();
        }

        public event Action OnToggleShowInfluenceVisualizer;

        public void ToggleShowInfluenceVisualizer()
        {
            OnToggleShowInfluenceVisualizer?.Invoke();
        }

        public event Action<Lens> OnPreviewInfluence;

        public void PreviewShowInfluenceEvent(Lens lens)
        {
            OnPreviewInfluence?.Invoke(lens);
        }
    }
}