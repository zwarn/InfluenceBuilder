using scriptableObjects.map;
using UnityEngine;

namespace map
{
    public class MapCreator : MonoBehaviour
    {
        [SerializeField] private TileType gras;
        [SerializeField] private TileType wall;
        [SerializeField] private TileType road;


        public TileType[] CreateMap(int width, int height)
        {
            TileType[] result = new TileType[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = y * width + x;

                    TileType terrain = gras;

                    if (x >= 30 && x <= 34 && y >= 15 && y <= 30)
                    {
                        terrain = road;
                    }
                    else if (x >= 4 && x <= 54 && y >= 20 && y <= 25)
                    {
                        terrain = wall;
                    }

                    result[index] = terrain;
                }
            }

            return result;
        }
    }
}