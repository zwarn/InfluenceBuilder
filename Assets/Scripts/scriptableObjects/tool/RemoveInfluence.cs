using influence;
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

        public override void Apply(int x, int y)
        {
            Layer? currentLayer = _showStatusController.CurrentLayer();
            if (currentLayer != null)
            {
                _influenceController.RemoveInfluence(Layer.Food, x, y, amount);
            }
        }
    }
}