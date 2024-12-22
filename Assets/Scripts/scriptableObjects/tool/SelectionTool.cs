using influence;
using lens;
using show;
using UnityEngine;
using Zenject;

namespace scriptableObjects.tool
{
    [CreateAssetMenu(fileName = "Selection", menuName = "tools/Selection", order = 0)]
    public class SelectionTool : SelectableTool
    {
        [Inject] private ShowStatusEvents _showStatusEvents;


        public override void Apply(int x, int y)
        {
            _showStatusEvents.SelectTileEvent(new Vector2Int(x, y));
        }
    }
}