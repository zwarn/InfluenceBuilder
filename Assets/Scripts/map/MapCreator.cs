using System;
using System.Linq;
using scriptableObjects.map;
using UnityEngine;

namespace map
{
    public class MapCreator : MonoBehaviour
    {
        [SerializeField] private TileType gras;
        [SerializeField] private TileType wall;
        [SerializeField] private TileType road;

        [SerializeField] private TileType[] tileTypes;


        public int[] CreateMap(int width, int height)
        {
            int[] result = new int[width * height];
            int grasIndex = Array.IndexOf(tileTypes, gras);
            int wallIndex = Array.IndexOf(tileTypes, wall);
            int roadIndex = Array.IndexOf(tileTypes, road);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = y * width + x;

                    int terrain = grasIndex;

                    if (x >= 30 && x <= 34 && y >= 15 && y <= 30)
                    {
                        terrain = roadIndex;
                    }
                    else if (x >= 4 && x <= 54 && y >= 20 && y <= 25)
                    {
                        terrain = wallIndex;
                    }

                    result[index] = terrain;
                }
            }

            return result;
        }

        public TileType[] GetTileTypes()
        {
            return tileTypes.ToArray();
        }
    }
}