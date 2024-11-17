using influence;
using UnityEngine;
using Zenject;

namespace scriptableObjects
{
    [CreateAssetMenu(fileName = "AddInfluence", menuName = "tools/AddInfluence", order = 0)]
    public class AddInfluence : SelectableTool
    {
        public int amount;

        [Inject] private InfluenceController _influenceController;

        public override void Apply(int x, int y)
        {
            _influenceController.AddInfluence(x, y, amount);
        }
    }
}