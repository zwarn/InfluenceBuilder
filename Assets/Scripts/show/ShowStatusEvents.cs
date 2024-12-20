using System;
using influence;
using lens;

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

        public event Action<Lens> OnShowLens;

        public void ShowLensEvent(Lens lens)
        {
            OnShowLens?.Invoke(lens);
        }
    }
}