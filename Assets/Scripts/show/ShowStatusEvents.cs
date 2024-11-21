using System;

namespace show
{
    public class ShowStatusEvents
    {
        public event Action<bool> OnShowTilemap;

        public void ShowTilemapEvent(bool show)
        {
            OnShowTilemap?.Invoke(show);
        }

        public event Action<bool> OnShowInfluence;

        public void ShowInfluenceEvent(bool show)
        {
            OnShowInfluence?.Invoke(show);
        }

        public event Action<bool> OnShowInfluenceVisualizer;

        public void ShowInfluenceVisualizerEvent(bool show)
        {
            OnShowInfluenceVisualizer?.Invoke(show);
        }
    }
}