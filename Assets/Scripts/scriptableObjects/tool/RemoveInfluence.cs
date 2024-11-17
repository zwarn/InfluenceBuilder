using influence;
using UnityEngine;
using Zenject;

namespace scriptableObjects.tool
{
    [CreateAssetMenu(fileName = "RemoveInfluence", menuName = "tools/RemoveInfluence", order = 0)]
    public class RemoveInfluence : SelectableTool
    {
        public int amount;

        [Inject] private InfluenceController _influenceController;

        public override void Apply(int x, int y)
        {
            _influenceController.RemoveInfluence(x, y, amount);
        }
    }
}