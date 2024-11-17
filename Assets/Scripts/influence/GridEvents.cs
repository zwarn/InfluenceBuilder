using System;

namespace influence
{
    public class GridEvents
    {
        public event Action OnGridUpdate;

        public void GridUpdateEvent()
        {
            OnGridUpdate?.Invoke();
        }
    }
}