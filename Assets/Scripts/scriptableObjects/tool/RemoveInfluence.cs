using influence;
using lens;
using show;
using UnityEngine;
using Zenject;

namespace scriptableObjects.tool
{
    [CreateAssetMenu(fileName = "RemoveInfluence", menuName = "tools/RemoveInfluence", order = 0)]
    public class RemoveInfluence : SelectableTool
    {
        public int amount;

        [Inject] private InfluenceController _influenceController;
        [Inject] private ShowStatusController _showStatusController;
        [Inject] private GridEvents _gridEvents;

        public override void Apply(int x, int y)
        {
            Lens currentLens = _showStatusController.CurrentLens();
            if (currentLens is LayerLens layerLens)
            {
                _influenceController.MinusInfluence(layerLens.GetLayer(), x, y, amount);
                _gridEvents.GridUpdateEvent();
            }
        }
    }
}