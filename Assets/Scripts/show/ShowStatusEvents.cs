using System;
using influence;

namespace show
{
    public class ShowStatusEvents
    {
        public event Action<bool> OnShowTilemap;

        public void ShowTilemapEvent(bool show)
        {
            OnShowTilemap?.Invoke(show);
        }

        public event Action<bool> OnShowInfluenceVisualizer;

        public void ShowInfluenceVisualizerEvent(bool show)
        {
            OnShowInfluenceVisualizer?.Invoke(show);
        }

        public event Action<Layer?> OnShowLayer;

        public void ShowLayerEvent(Layer? layer)
        {
            OnShowLayer?.Invoke(layer);
        }
    }
}