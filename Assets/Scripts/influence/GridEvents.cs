using System;
using UnityEngine;

namespace influence
{
    public class GridEvents
    {
        public event Action OnGridUpdate;

        public void GridUpdateEvent()
        {
            OnGridUpdate?.Invoke();
        }

        public event Action<Vector2Int> OnMapTileChanged;

        public void MapTileChangedEvent(int x, int y)
        {
            OnMapTileChanged?.Invoke(new Vector2Int(x, y));
        }
    }
}