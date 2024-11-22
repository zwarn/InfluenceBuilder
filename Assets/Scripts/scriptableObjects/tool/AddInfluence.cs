using influence;
using show;
using UnityEngine;
using Zenject;

namespace scriptableObjects.tool
{
    [CreateAssetMenu(fileName = "AddInfluence", menuName = "tools/AddInfluence", order = 0)]
    public class AddInfluence : SelectableTool
    {
        public int amount;

        [Inject] private InfluenceController _influenceController;
        [Inject] private ShowStatusController _showStatusController;

        public override void Apply(int x, int y)
        {
            Layer? currentLayer = _showStatusController.CurrentLayer();
            if (currentLayer != null)
            {
                _influenceController.AddInfluence(currentLayer.Value, x, y, amount);
            }
        }
    }
}