using influence;
using map;
using scriptableObjects.map;
using UnityEngine;
using Zenject;

namespace scriptableObjects.tool
{
    [CreateAssetMenu(fileName = "Terrain", menuName = "tools/ChangeTerrain", order = 0)]
    public class ChangeTerrain : SelectableTool
    {
        public TileType target;

        [Inject] private MapController _mapController;

        public override void Apply(int x, int y)
        {
            _mapController.ChangeTile(x, y, target);
        }
    }
}