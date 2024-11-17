using influence;
using UnityEngine;

namespace scriptableObjects
{
    [CreateAssetMenu(fileName = "RemoveInfluence", menuName = "tools/RemoveInfluence", order = 0)]
    public class RemoveInfluence : SelectableTool
    {
        public int amount;

        private InfluenceController _influenceController;

        public override void Apply(int x, int y)
        {
            _influenceController.RemoveInfluence(x, y, amount);
        }
    }
}